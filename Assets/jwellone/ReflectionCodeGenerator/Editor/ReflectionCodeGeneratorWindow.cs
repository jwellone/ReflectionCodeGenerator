using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

#nullable enable

namespace jwelloneEditor
{
    public class ReflectionCodeGeneratorWindow : EditorWindow
    {
        int _tabIndex;
        string _codeText = string.Empty;
        string _searchFieldText = string.Empty;
        Vector2 _scrollPositionTypeArea;
        Vector2 _scrollPositionRightArea;
        SearchField? _searchField;
        GUIContent? _iconTextButton;
        GUIContent? _iconSave;
        Type? _targetType;
        readonly List<Type> _types = new List<Type>();
        readonly List<Type> _elements = new List<Type>();
        readonly List<(bool, MethodInfo)> _methodInfos = new List<(bool, MethodInfo)>();
        readonly List<(bool, FieldInfo)> _fieldInfos = new List<(bool, FieldInfo)>();
        readonly string[] _tabNames = new[] { "Item", "Code" };

        [MenuItem("jwellone/window/ReflectionCodeGenerator")]
        static void OnOpen()
        {
            var window = GetWindow(typeof(ReflectionCodeGeneratorWindow));
            window.titleContent = EditorGUIUtility.TrTextContent("Code Generator", "Internal code output.", "d_cs Script Icon");
        }

        void OnEnable()
        {
            var list = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t =>
                {
                    if (t.IsAbstract || t.IsInterface || !(t.IsClass || t.IsValueType && !t.IsPrimitive && !t.IsEnum))
                    {
                        return false;
                    }
                    return true;
                })
                .ToList();

            _types.AddRange(list);

            _searchField = new SearchField();

            _iconTextButton = EditorGUIUtility.TrIconContent("d_TextAsset Icon");
            _iconSave = EditorGUIUtility.TrIconContent("d_SaveAs");

            UpdateElements();
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal();

            OnDrawLeftArea();
            OnDrawRightArea();

            GUILayout.EndHorizontal();
        }

        void OnDrawLeftArea()
        {
            var drawWidth = position.width / 2.5f;
            GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(drawWidth));

            var serchFieldText = _searchField!.OnToolbarGUI(_searchFieldText);
            if (serchFieldText != _searchFieldText)
            {
                _searchFieldText = serchFieldText;
                UpdateElements();
            }

            _scrollPositionTypeArea = EditorGUILayout.BeginScrollView(_scrollPositionTypeArea);
            var height = 16f;
            var margin = 2f;
            var top = _scrollPositionTypeArea.y - (height + margin);
            var bottom = _scrollPositionTypeArea.y + position.height;
            var y = 0f;
            var layoutHeight = GUILayout.Height(height);
            var layoutTextFieldWidth = GUILayout.Width(drawWidth - 32 - 22);
            for (var i = 0; i < _elements.Count; ++i)
            {
                if (top <= y && bottom >= y)
                {
                    var target = _elements[i];
                    GUILayout.BeginHorizontal(layoutHeight);
                    GUILayout.TextField(target.FullName, layoutTextFieldWidth);
                    if (GUILayout.Button(_iconTextButton, GUILayout.Width(32), GUILayout.Height(18)))
                    {
                        _scrollPositionRightArea = Vector2.zero;
                        SetTarget(target);
                    }
                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.Label("", layoutHeight);
                }

                y += height + margin;
            }

            EditorGUILayout.EndScrollView();

            GUILayout.EndVertical();
        }

        void OnDrawRightArea()
        {
            GUILayout.BeginVertical(GUI.skin.box);

            GUILayout.BeginHorizontal();
            var tabIndex = GUILayout.Toolbar(_tabIndex, _tabNames, new GUIStyle(EditorStyles.toolbarButton), GUI.ToolbarButtonSize.FitToContents);
            if (tabIndex != _tabIndex)
            {
                _tabIndex = tabIndex;
                _scrollPositionRightArea = Vector2.zero;
                if (_tabIndex == 1)
                {
                    UpdateCodeText();
                }
            }

            GUI.enabled = !(_targetType == null || string.IsNullOrEmpty(_codeText));
            if (GUILayout.Button(_iconSave, GUILayout.Width(32)))
            {
                OutputCode();
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.Space(4);

            _scrollPositionRightArea = EditorGUILayout.BeginScrollView(_scrollPositionRightArea);

            if (_tabIndex == 0)
            {
                for (var i = 0; i < _fieldInfos.Count; ++i)
                {
                    var info = _fieldInfos[i];
                    info.Item1 = GUILayout.Toggle(info.Item1, TypeNameConvert.Convert(info.Item2.FieldType) + " " + info.Item2.Name);
                    _fieldInfos[i] = info;
                }

                for (var i = 0; i < _methodInfos.Count; ++i)
                {
                    var info = _methodInfos[i];
                    info.Item1 = GUILayout.Toggle(info.Item1, TypeToTextConvert.MthodInfoToFullName(info.Item2));
                    _methodInfos[i] = info;
                }
            }
            else
            {
                GUILayout.TextArea(_codeText);
            }

            EditorGUILayout.EndScrollView();

            GUILayout.EndVertical();
        }

        void SetTarget(Type target)
        {
            _targetType = target;
            _fieldInfos.Clear();
            _methodInfos.Clear();

            var bindingFlagsTable = new[]
            {
                BindingFlags.NonPublic | BindingFlags.Instance,
                BindingFlags.NonPublic | BindingFlags.Static
            };

            foreach (var bindingFlags in bindingFlagsTable)
            {
                var methods = target.GetMethods(bindingFlags);
                foreach (var method in methods)
                {
                    _methodInfos.Add((true, method));
                }

                var fields = target.GetFields(bindingFlags);
                foreach (var field in fields)
                {
                    if (field.Name.Contains("k__BackingField"))
                    {
                        continue;
                    }
                    _fieldInfos.Add((true, field));
                }
            }

            UpdateCodeText();
        }

        void UpdateCodeText()
        {
            if (_targetType == null)
            {
                _codeText = string.Empty;
                return;
            }

            var fieldInfos = _fieldInfos.Where(f => f.Item1).Select(f => f.Item2);
            var methodInfos = _methodInfos.Where(m => m.Item1).Select(m => m.Item2);
            _codeText = TypeToTextConvert.GetCodeText(_targetType!, fieldInfos, methodInfos);
        }

        void UpdateElements()
        {
            _elements.Clear();

            if (string.IsNullOrEmpty(_searchFieldText))
            {
                _elements.AddRange(_types);
            }
            else
            {
                foreach (var type in _types)
                {
                    if (type.FullName.Contains(_searchFieldText))
                    {
                        _elements.Add(type);
                    }
                }
            }

            _elements.Sort((a, b) => a.FullName.CompareTo(b.FullName));
        }

        void OutputCode()
        {
            UpdateCodeText();

            var fileName = TypeToTextConvert.MakeClassName(_targetType!) + ".cs";
            var guids = AssetDatabase.FindAssets(string.Format("t:script {0}", Path.GetFileNameWithoutExtension(fileName)));
            var relativePath = (guids.Length > 0) ?
                AssetDatabase.GUIDToAssetPath(guids[0]).Replace("Assets/", string.Empty) :
                fileName;

            var fullPath = Path.Combine(Application.dataPath, relativePath);
            using (var stream = new StreamWriter(fullPath, false, Encoding.UTF8))
            {
                stream.NewLine = "\n";
                stream.Write(_codeText.Replace("\r\n", "\n"));
                Debug.Log($"{fullPath} generated.");
            }

            if (guids.Length > 0)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            }
        }
    }
}