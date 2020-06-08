using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;
using System;

namespace UniVRM10
{
    [CustomEditor(typeof(VrmHumanoid))]
    public class VrmHumanoidEditor : Editor
    {
        const int LABEL_WIDTH = 100;

        VrmHumanoid m_target;

        SerializedProperty m_Hips;
        #region leg
        SerializedProperty m_LeftUpperLeg;
        SerializedProperty m_RightUpperLeg;
        SerializedProperty m_LeftLowerLeg;
        SerializedProperty m_RightLowerLeg;
        SerializedProperty m_LeftFoot;
        SerializedProperty m_RightFoot;
        SerializedProperty m_LeftToes;
        SerializedProperty m_RightToes;

        #endregion

        #region spine
        SerializedProperty m_Spine;
        SerializedProperty m_Chest;
        SerializedProperty m_UpperChest;
        SerializedProperty m_Neck;
        SerializedProperty m_Head;
        SerializedProperty m_LeftEye;
        SerializedProperty m_RightEye;
        SerializedProperty m_Jaw;

        #endregion

        #region arm
        SerializedProperty m_LeftShoulder;
        SerializedProperty m_RightShoulder;
        SerializedProperty m_LeftUpperArm;
        SerializedProperty m_RightUpperArm;
        SerializedProperty m_LeftLowerArm;
        SerializedProperty m_RightLowerArm;
        SerializedProperty m_LeftHand;
        SerializedProperty m_RightHand;

        #endregion

        #region fingers
        SerializedProperty m_LeftThumbProximal;
        SerializedProperty m_LeftThumbIntermediate;
        SerializedProperty m_LeftThumbDistal;
        SerializedProperty m_LeftIndexProximal;
        SerializedProperty m_LeftIndexIntermediate;
        SerializedProperty m_LeftIndexDistal;
        SerializedProperty m_LeftMiddleProximal;
        SerializedProperty m_LeftMiddleIntermediate;
        SerializedProperty m_LeftMiddleDistal;
        SerializedProperty m_LeftRingProximal;
        SerializedProperty m_LeftRingIntermediate;
        SerializedProperty m_LeftRingDistal;
        SerializedProperty m_LeftLittleProximal;
        SerializedProperty m_LeftLittleIntermediate;
        SerializedProperty m_LeftLittleDistal;
        SerializedProperty m_RightThumbProximal;
        SerializedProperty m_RightThumbIntermediate;
        SerializedProperty m_RightThumbDistal;
        SerializedProperty m_RightIndexProximal;
        SerializedProperty m_RightIndexIntermediate;
        SerializedProperty m_RightIndexDistal;
        SerializedProperty m_RightMiddleProximal;
        SerializedProperty m_RightMiddleIntermediate;
        SerializedProperty m_RightMiddleDistal;
        SerializedProperty m_RightRingProximal;
        SerializedProperty m_RightRingIntermediate;
        SerializedProperty m_RightRingDistal;
        SerializedProperty m_RightLittleProximal;
        SerializedProperty m_RightLittleIntermediate;
        SerializedProperty m_RightLittleDistal;

        #endregion

        void OnEnable()
        {
            m_target = target as VrmHumanoid;
            m_Hips = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.Hips)}");

            #region legs
            m_LeftUpperLeg = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftUpperLeg)}");
            m_RightUpperLeg = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightUpperLeg)}");
            m_LeftLowerLeg = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftLowerLeg)}");
            m_RightLowerLeg = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightLowerLeg)}");
            m_LeftFoot = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftFoot)}");
            m_RightFoot = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightFoot)}");
            m_LeftToes = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftToes)}");
            m_RightToes = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightToes)}");
            #endregion

            #region spine
            m_Spine = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.Spine)}");
            m_Chest = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.Chest)}");
            m_UpperChest = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.UpperChest)}");
            m_Neck = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.Neck)}");
            m_Head = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.Head)}");
            m_LeftEye = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftEye)}");
            m_RightEye = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightEye)}");
            m_Jaw = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.Jaw)}");

            #endregion

            #region arm
            m_LeftShoulder = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftShoulder)}");
            m_RightShoulder = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightShoulder)}");
            m_LeftUpperArm = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftUpperArm)}");
            m_RightUpperArm = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightUpperArm)}");
            m_LeftLowerArm = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftLowerArm)}");
            m_RightLowerArm = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightLowerArm)}");
            m_LeftHand = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftHand)}");
            m_RightHand = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightHand)}");

            #endregion

            #region fingers
            m_LeftThumbProximal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftThumbProximal)}");
            m_LeftThumbIntermediate = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftThumbIntermediate)}");
            m_LeftThumbDistal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftThumbDistal)}");
            m_LeftIndexProximal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftIndexProximal)}");
            m_LeftIndexIntermediate = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftIndexIntermediate)}");
            m_LeftIndexDistal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftIndexDistal)}");
            m_LeftMiddleProximal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftMiddleProximal)}");
            m_LeftMiddleIntermediate = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftMiddleIntermediate)}");
            m_LeftMiddleDistal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftMiddleDistal)}");
            m_LeftRingProximal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftRingProximal)}");
            m_LeftRingIntermediate = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftRingIntermediate)}");
            m_LeftRingDistal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftRingDistal)}");
            m_LeftLittleProximal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftLittleProximal)}");
            m_LeftLittleIntermediate = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftLittleIntermediate)}");
            m_LeftLittleDistal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.LeftLittleDistal)}");
            m_RightThumbProximal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightThumbProximal)}");
            m_RightThumbIntermediate = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightThumbIntermediate)}");
            m_RightThumbDistal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightThumbDistal)}");
            m_RightIndexProximal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightIndexProximal)}");
            m_RightIndexIntermediate = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightIndexIntermediate)}");
            m_RightIndexDistal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightIndexDistal)}");
            m_RightMiddleProximal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightMiddleProximal)}");
            m_RightMiddleIntermediate = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightMiddleIntermediate)}");
            m_RightMiddleDistal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightMiddleDistal)}");
            m_RightRingProximal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightRingProximal)}");
            m_RightRingIntermediate = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightRingIntermediate)}");
            m_RightRingDistal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightRingDistal)}");
            m_RightLittleProximal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightLittleProximal)}");
            m_RightLittleIntermediate = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightLittleIntermediate)}");
            m_RightLittleDistal = serializedObject.FindProperty($"m_{nameof(VrmHumanoid.RightLittleDistal)}");
            #endregion
        }


        //         static GameObject ObjectField(GameObject obj)
        //         {
        //             return (GameObject)EditorGUILayout.ObjectField(obj, typeof(GameObject), true);
        //         }

        //         static GameObject ObjectField(string label, GameObject obj)
        //         {
        //             return (GameObject)EditorGUILayout.ObjectField(label, obj, typeof(GameObject), true);
        //         }


        //         static void BoneField(HumanBodyBones bone)
        //         {
        //             EditorGUILayout.BeginHorizontal();
        //             EditorGUILayout.LabelField(bone.ToString(), GUILayout.Width(LABEL_WIDTH));
        //             // bones[(int)bone] = ObjectField(bones[(int)bone]);
        //             EditorGUILayout.EndHorizontal();
        //         }

        //         bool m_handFoldout;
        //         bool m_settingsFoldout;

        struct Horizontal : IDisposable
        {
            public static Horizontal Using()
            {
                EditorGUILayout.BeginHorizontal();
                return default;
            }
            public void Dispose()
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        static bool HorizontalFields(string label, params SerializedProperty[] props)
        {
            var updated = false;
            try
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label(label, GUILayout.Width(LABEL_WIDTH));
                GUILayout.FlexibleSpace();

                foreach (var prop in props)
                {
                    if (EditorGUILayout.PropertyField(prop, GUIContent.none, true, GUILayout.MinWidth(100)))
                    {
                        updated = true;
                    }
                }
            }
            finally
            {
                EditorGUILayout.EndHorizontal();
            }
            return updated;
        }

        static bool s_spineFold;
        static bool s_legFold;
        static bool s_armFold;
        static bool s_fingerFold;

        public override void OnInspectorGUI()
        {
            // prefer
            var updated = false;
            serializedObject.Update();

            // create avatar
            if (GUILayout.Button("Create UnityEngine.Avatar"))
            {
                var prefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(m_target);
                var prefabPath = AssetDatabase.GetAssetPath(prefabRoot);
                var path = (string.IsNullOrEmpty(prefabPath))
                    ? string.Format("Assets/{0}.asset", m_target.gameObject.name)
                    : string.Format("{0}/{1}.asset", Path.GetDirectoryName(prefabPath), Path.GetFileNameWithoutExtension(prefabPath))
                    ;
                path = EditorUtility.SaveFilePanel(
                        "Save avatar",
                        Path.GetDirectoryName(path),
                        string.Format("{0}.avatar.asset", serializedObject.targetObject.name),
                        "asset");
                if (!string.IsNullOrEmpty(path))
                {
                    var avatar = m_target.CreateAvatar();
                    if (avatar != null)
                    {
                        var unityPath = UnityPath.FromFullpath(path);
                        avatar.name = "avatar";
                        Debug.LogFormat("Create avatar {0}", unityPath);
                        AssetDatabase.CreateAsset(avatar, unityPath.Value);
                        AssetDatabase.ImportAsset(unityPath.Value);

                        // replace
                        var animator = m_target.GetComponent<Animator>();
                        animator.avatar = avatar;

                        Selection.activeObject = avatar;
                    }
                }
            }

            if (EditorGUILayout.PropertyField(m_Hips)) updated = true;

            s_spineFold = EditorGUILayout.Foldout(s_spineFold, "Spine");
            if (s_spineFold)
            {
                if (EditorGUILayout.PropertyField(m_Spine)) updated = true;
                if (EditorGUILayout.PropertyField(m_Chest)) updated = true;
                if (EditorGUILayout.PropertyField(m_UpperChest)) updated = true;
                if (EditorGUILayout.PropertyField(m_Neck)) updated = true;
                if (EditorGUILayout.PropertyField(m_Head)) updated = true;
                if (EditorGUILayout.PropertyField(m_Jaw)) updated = true;
                if (HorizontalFields("Eye", m_LeftEye, m_RightEye)) updated = true;
            }

            s_legFold = EditorGUILayout.Foldout(s_legFold, "Leg");
            if (s_legFold)
            {
                if (HorizontalFields("UpperLeg", m_LeftUpperLeg, m_RightUpperLeg)) updated = true;
                if (HorizontalFields("LowerLeg", m_LeftLowerLeg, m_RightLowerLeg)) updated = true;
                if (HorizontalFields("Foot", m_LeftFoot, m_RightFoot)) updated = true;
                if (HorizontalFields("Toes", m_LeftToes, m_RightToes)) updated = true;
            }

            s_armFold = EditorGUILayout.Foldout(s_armFold, "Arm");
            if (s_armFold)
            {
                if (HorizontalFields("Shoulder", m_LeftShoulder, m_RightShoulder)) updated = true;
                if (HorizontalFields("UpperArm", m_LeftUpperArm, m_RightUpperArm)) updated = true;
                if (HorizontalFields("LowerArm", m_LeftLowerArm, m_RightLowerArm)) updated = true;
                if (HorizontalFields("Hand", m_LeftHand, m_RightHand)) updated = true;
            }

            s_fingerFold = EditorGUILayout.Foldout(s_fingerFold, "Finger");
            if (s_fingerFold)
            {
                if (HorizontalFields("LeftThumb", m_LeftThumbProximal, m_LeftThumbIntermediate, m_LeftThumbDistal)) updated = true;
                if (HorizontalFields("LeftIndex", m_LeftIndexProximal, m_LeftIndexIntermediate, m_LeftIndexDistal)) updated = true;
                if (HorizontalFields("LeftMiddle", m_LeftMiddleProximal, m_LeftMiddleIntermediate, m_LeftMiddleDistal)) updated = true;
                if (HorizontalFields("LeftRing", m_LeftRingProximal, m_LeftRingIntermediate, m_LeftRingDistal)) updated = true;
                if (HorizontalFields("LeftLittle", m_LeftLittleProximal, m_LeftLittleIntermediate, m_LeftLittleDistal)) updated = true;
                if (HorizontalFields("RightThumb", m_RightThumbProximal, m_RightThumbIntermediate, m_RightThumbDistal)) updated = true;
                if (HorizontalFields("RightIndex", m_RightIndexProximal, m_RightIndexIntermediate, m_RightIndexDistal)) updated = true;
                if (HorizontalFields("RightMiddle", m_RightMiddleProximal, m_RightMiddleIntermediate, m_RightMiddleDistal)) updated = true;
                if (HorizontalFields("RightRing", m_RightRingProximal, m_RightRingIntermediate, m_RightRingDistal)) updated = true;
                if (HorizontalFields("RightLittle", m_RightLittleProximal, m_RightLittleIntermediate, m_RightLittleDistal)) updated = true;
            }

            if (updated)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        //         private void OnSceneGUI()
        //         {
        //             var bones = m_target.Bones;
        //             if (bones != null)
        //             {
        //                 for (int i = 0; i < bones.Length; ++i)
        //                 {
        //                     DrawBone((HumanBodyBones)i, bones[i]);
        //                 }
        //                 foreach (var x in m_bones)
        //                 {
        //                     x.Draw();
        //                 }
        //             }
        //         }
    }
}
