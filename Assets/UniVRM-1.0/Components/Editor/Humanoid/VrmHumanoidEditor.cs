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

        static void HorizontalFields(string label, params SerializedProperty[] props)
        {
            try
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label(label, GUILayout.Width(LABEL_WIDTH));
                GUILayout.FlexibleSpace();

                foreach (var prop in props)
                {
                    EditorGUILayout.PropertyField(prop, GUIContent.none, true, GUILayout.MinWidth(100));
                }
            }
            finally
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        static bool s_spineFold;
        static bool s_legFold;
        static bool s_armFold;
        static bool s_fingerFold;

        struct Validation
        {
            public readonly string Message;
            public readonly MessageType MessageType;

            public Validation(string message, MessageType messageType)
            {
                Message = message;
                MessageType = messageType;
            }
        }

        IEnumerable<Validation> Required(params SerializedProperty[] props)
        {
            foreach (var prop in props)
            {
                if (prop.objectReferenceValue is null)
                {
                    var name = prop.name;
                    if (name.StartsWith("m_"))
                    {
                        name = name.Substring(2);
                    }
                    yield return new Validation($"{name} is Required", MessageType.Error);
                }
            }
        }

        IEnumerable<Validation> Validate()
        {
            foreach (var validation in Required(
                m_Hips, m_Spine, m_Head,
                m_LeftUpperLeg, m_LeftLowerLeg, m_LeftFoot,
                m_RightUpperLeg, m_RightLowerLeg, m_RightFoot,
                m_LeftUpperArm, m_LeftLowerArm, m_LeftHand,
                m_RightUpperArm, m_RightLowerArm, m_RightHand
            ))
            {
                yield return validation;
            }
        }

        public override void OnInspectorGUI()
        {
            foreach (var validation in Validate())
            {
                EditorGUILayout.HelpBox(validation.Message, validation.MessageType);
            }

            // prefer
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_Hips);

            s_spineFold = EditorGUILayout.Foldout(s_spineFold, "Spine");
            if (s_spineFold)
            {
                EditorGUILayout.PropertyField(m_Spine);
                EditorGUILayout.PropertyField(m_Chest);
                EditorGUILayout.PropertyField(m_UpperChest);
                EditorGUILayout.PropertyField(m_Neck);
                EditorGUILayout.PropertyField(m_Head);
                EditorGUILayout.PropertyField(m_Jaw);
                HorizontalFields("Eye", m_LeftEye, m_RightEye);
            }

            s_legFold = EditorGUILayout.Foldout(s_legFold, "Leg");
            if (s_legFold)
            {
                HorizontalFields("UpperLeg", m_LeftUpperLeg, m_RightUpperLeg);
                HorizontalFields("LowerLeg", m_LeftLowerLeg, m_RightLowerLeg);
                HorizontalFields("Foot", m_LeftFoot, m_RightFoot);
                HorizontalFields("Toes", m_LeftToes, m_RightToes);
            }

            s_armFold = EditorGUILayout.Foldout(s_armFold, "Arm");
            if (s_armFold)
            {
                HorizontalFields("Shoulder", m_LeftShoulder, m_RightShoulder);
                HorizontalFields("UpperArm", m_LeftUpperArm, m_RightUpperArm);
                HorizontalFields("LowerArm", m_LeftLowerArm, m_RightLowerArm);
                HorizontalFields("Hand", m_LeftHand, m_RightHand);
            }

            s_fingerFold = EditorGUILayout.Foldout(s_fingerFold, "Finger");
            if (s_fingerFold)
            {
                HorizontalFields("LeftThumb", m_LeftThumbProximal, m_LeftThumbIntermediate, m_LeftThumbDistal);
                HorizontalFields("LeftIndex", m_LeftIndexProximal, m_LeftIndexIntermediate, m_LeftIndexDistal);
                HorizontalFields("LeftMiddle", m_LeftMiddleProximal, m_LeftMiddleIntermediate, m_LeftMiddleDistal);
                HorizontalFields("LeftRing", m_LeftRingProximal, m_LeftRingIntermediate, m_LeftRingDistal);
                HorizontalFields("LeftLittle", m_LeftLittleProximal, m_LeftLittleIntermediate, m_LeftLittleDistal);
                HorizontalFields("RightThumb", m_RightThumbProximal, m_RightThumbIntermediate, m_RightThumbDistal);
                HorizontalFields("RightIndex", m_RightIndexProximal, m_RightIndexIntermediate, m_RightIndexDistal);
                HorizontalFields("RightMiddle", m_RightMiddleProximal, m_RightMiddleIntermediate, m_RightMiddleDistal);
                HorizontalFields("RightRing", m_RightRingProximal, m_RightRingIntermediate, m_RightRingDistal);
                HorizontalFields("RightLittle", m_RightLittleProximal, m_RightLittleIntermediate, m_RightLittleDistal);
            }

            serializedObject.ApplyModifiedProperties();

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
