using System.Collections.Generic;
using System.Linq;
using GAS.GameplayTag;
using GAS.GameplayTag.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class TagEditorWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset _visualTreeAsset = default;
    [SerializeField]
    private GameplayTagsAsset _tagsAsset;

    private TreeView _treeView;
    private TextField _newTagField;
    private TextField _assetPathField;
    private Label _outputLabel;

    private int selTagId = 0;

    [MenuItem("GAS/TagEditorWindow")]
    public static void ShowExample()
    {
        TagEditorWindow wnd = GetWindow<TagEditorWindow>();
        wnd.titleContent = new GUIContent("TagEditorWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Instantiate UXML
        VisualElement labelFromUXML = _visualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        _treeView = root.Q<TreeView>();
        _treeView.selectionChanged += items =>
        {
            selTagId = _treeView.GetIdForIndex(_treeView.selectedIndex);
            // Debug.Log(selTagId);
        };
        root.Q<Button>("AddTag").clicked += OnClickAdd;
        root.Q<Button>("RemoveTag").clicked += OnClickRemove;
        root.Q<Button>("BtnLoadAsset").clicked += OnClickLoadTagAsset;

        _assetPathField = root.Q<TextField>("AssetPathTF");
        _newTagField = root.Q<TextField>("NewTagTF");
        
        _outputLabel = root.Q<Label>("OutputLabel");
        RefreshTreeView();
    }

    private void RefreshTreeView()
    {
        if (_tagsAsset == null)
        {
            return;
        }
        _treeView.SetRootItems(new List<TreeViewItemData<string>>());
        ParseTagNode(_tagsAsset._rootNode, -1);
    }
    
    private void ParseTagNode(GameplayTagNode node, int parentId)
    {
        var item = new TreeViewItemData<string>(node.tag.hashId, node.nodeName);
        _treeView.AddItem(item, parentId);
        foreach (var child in node.children)
        {
            ParseTagNode(child, item.id);
        }
    }

    private void OnClickAdd()
    {
        var tagName = _newTagField.value;
        _tagsAsset.AddTag(tagName);
        _tagsAsset.ReBuildNodes();
        _newTagField.value = "";
        RefreshTreeView();
    }

    private void OnClickRemove()
    {
        _tagsAsset.RemoveTag(selTagId);
        _tagsAsset.ReBuildNodes();
        RefreshTreeView();
    }

    private void OnClickLoadTagAsset()
    {
        var path = _assetPathField.value;
        _tagsAsset = AssetDatabase.LoadAssetAtPath<GameplayTagsAsset>(path);
        if (_tagsAsset == null)
        {
            var msg = $"LoadAssetAtPath<GameplayTagsAsset> failed: {path}";
            LogOutput(msg);
        }
        else
        {
            var msg = $"LoadAssetAtPath<GameplayTagsAsset> success: {path}";
            LogOutput(msg);
        }
        _tagsAsset.ReBuildNodes();
        RefreshTreeView();
    }
    
    private void LogOutput(string msg)
    {
        _outputLabel.text = msg;
    }
}
