using MelonLoader;
using UnityEngine;
using Il2CppSystem.Collections.Generic;
using System;
using HeroCameraName;
using Item;
using DataHelper;

namespace GRMod
{
    public static class BuildInfo
    {
        public const string Name = "GRMod"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "Gunfire Reborn Mod. Original Author pentium1131 And Hkl146. Modified By zhang"; // Description for the Mod.  (Set as null if none)
        public const string Author = "zhang"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "2.1.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = "https://github.com/zhang0281/GRMod/releases/latest"; // Download Link for the Mod.  (Set as null if none)
    }

    public class GRMod : MelonMod
    {
        public static bool shownpc = false;
        public static KeyCode showUIKey = KeyCode.Home;
        public static KeyCode needinitKey = KeyCode.Insert;
        public static bool showUI = false;
        public static KeyCode autoaimKey = KeyCode.F1;
        public static bool autoaim = false;
        public static KeyCode weaponEnhanceKey = KeyCode.F2;
        public static bool weaponEnhance = false;
        public static KeyCode playerEnhanceKey = KeyCode.F3;
        public static bool playerEnhance = false;
        public static KeyCode shownpcKey = KeyCode.LeftAlt;
        private float originJumpHeight;
        private float originSpeed;
        public static bool needinit = true;

        /**
         * 快捷键开关相关代码
         */
        public void SwitchHotKey()
        {
            // 重新初始化开关
            if (Input.GetKeyUp(needinitKey))
            {
                needinit = !needinit;
            }
            // 辅助瞄准开关
            if (Input.GetKeyUp(autoaimKey))
            {
                autoaim = !autoaim;
                MelonLogger.Msg("辅助瞄准已" + (autoaim ? "开启" : "关闭"));
            }
            // 显示UI开关
            if (Input.GetKeyUp(showUIKey))
            {
                showUI = !showUI;
                MelonLogger.Msg("UI已" + (showUI ? "开启" : "关闭"));
            }
            // 武器增强开关
            if (Input.GetKeyUp(weaponEnhanceKey))
            {
                weaponEnhance = !weaponEnhance;
                MelonLogger.Msg("武器增强已" + (weaponEnhance ? "开启" : "关闭"));
            }
            // 透视开关
            if (Input.GetKey(shownpcKey))
                shownpc = true;
            else
                shownpc = false;
            // 玩家增强开关
            if (Input.GetKeyUp(playerEnhanceKey))
            {
                playerEnhance = !playerEnhance;
                MelonLogger.Msg("玩家增强已" + (playerEnhance ? "开启" : "关闭"));
            }
        }

        public override void OnUpdate()
        {
            try
            {
                // 初始化
                if ((needinit) && HeroCameraManager.HeroObj != null && HeroCameraManager.HeroObj.BulletPreFormCom != null && HeroCameraManager.HeroObj.BulletPreFormCom.weapondict != null)
                {
                    shownpc = false;
                    showUI = false;
                    autoaim = false;
                    weaponEnhance = false;
                    playerEnhance = false;

                    needinit = false;
                }
                // 设置快捷键
                SwitchHotKey();
                // 武器增强
                if (weaponEnhance && HeroCameraManager.HeroObj != null && HeroCameraManager.HeroObj.BulletPreFormCom != null && HeroCameraManager.HeroObj.BulletPreFormCom.weapondict != null)
                {
                    foreach (KeyValuePair<int, WeaponPerformanceObj> weapon in HeroCameraManager.HeroObj.BulletPreFormCom.weapondict)
                    {
                        // 弹药
                        weapon.value.ModifyBulletInMagzine(200, 200);
                        weapon.value.ReloadBulletImmediately();
                        //武器精确度
                        if (weapon.value.WeaponAttr.Stability[0] != 10000)
                        {
                            weapon.value.WeaponAttr.Stability[0] = 100000;
                        }
                        //武器稳定性
                        if (weapon.value.WeaponAttr.Accuracy[0] != 10000)
                        {
                            weapon.value.WeaponAttr.Accuracy[0] = 100000;
                        }
                        // 穿透 效果存疑
                        weapon.value.WeaponAttr.Pierce = 100;
                        //爆炸范围(会影响爆炸类武器、火标和电手套)
                        if (weapon.value.WeaponAttr.Radius > 0f && weapon.value.WeaponAttr.Radius < 9f)
                        {
                            weapon.value.WeaponAttr.Radius = 9f;
                        }
                        // 射程
                        if (weapon.value.WeaponAttr.AttDis != 9999f)
                        {
                            weapon.value.WeaponAttr.AttDis = 9999f;
                        }
                        // 换弹时间
                        if (weapon.value.WeaponAttr.FillTime[0] != 5)
                        {
                            weapon.value.WeaponAttr.FillTime[0] = 5;
                        }
                        // 子弹速度
                        if (weapon.value.WeaponAttr.BulletSpeed >= 50f && weapon.value.WeaponAttr.BulletSpeed != 55f || weapon.value.WeaponAttr.BulletSpeed == 30f)
                        {
                            if (weapon.value.WeaponAttr.BulletSpeed != 100f)
                            {
                                weapon.value.WeaponAttr.BulletSpeed = 500f;
                            }
                        }
                        else if (weapon.value.WeaponAttr.BulletSpeed == 30f && weapon.value.WeaponAttr.BulletSpeed != 60f)
                        {
                            weapon.value.WeaponAttr.BulletSpeed = 200f;
                        }
                    }
                }
                // 玩家增强
                if (HeroCameraManager.HeroObj != null)
                {
                    if (originJumpHeight == 0f)
                    {
                        originJumpHeight = HeroMoveManager.HMMJS.jumping.baseHeight;
                    }
                    if (originSpeed == 0f)
                    {
                        originSpeed = HeroMoveManager.HMMJS.maxForwardSpeed;
                    }
                    if (playerEnhance)
                    {
                        HeroMoveManager.HMMJS.jumping.baseHeight = 64f / (HeroMoveManager.HMMJS.movement.gravity * 2f);
                        HeroMoveManager.HMMJS.maxForwardSpeed = (HeroMoveManager.HMMJS.maxBackwardsSpeed = (HeroMoveManager.HMMJS.maxSidewaysSpeed = 10f));
                    }
                    else
                    {
                        HeroMoveManager.HMMJS.jumping.baseHeight = originJumpHeight;
                        HeroMoveManager.HMMJS.maxForwardSpeed = (HeroMoveManager.HMMJS.maxBackwardsSpeed = (HeroMoveManager.HMMJS.maxSidewaysSpeed = originSpeed));
                    }
                }
                // 辅助瞄准
                if (autoaim && (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)))
                {
                    List<NewPlayerObject> monsters = NewPlayerManager.GetMonsters();

                    if (monsters != null)
                    {

                        Vector3 campos = CameraManager.MainCamera.position;
                        Transform nearmons = null;
                        Transform monsterTransform = null;
                        float neardis = 500f;

                        foreach (var monster in monsters)
                        {

                            if (monster.BodyPartCom == null)
                                continue;

                            if (monster.BloodBarCom == null)
                                continue;

                            if (monster.BloodBarCom.BloodBar == null)
                                continue;

                            monsterTransform = monster.BodyPartCom.GetWeakTrans();

                            if (monsterTransform == null)
                                continue;

                            Vector3 vector = CameraManager.MainCameraCom.WorldToViewportPoint(monsterTransform.position);

                            bool flag = vector.x >= 0.45f && vector.x <= 0.55f;
                            flag = flag && vector.y >= 0.45f && vector.y <= 0.55f;
                            flag = flag && vector.z > 0f;
                            if (flag)
                            {
                                // 计算屏幕角度
                                vector.y = 0f;
                                vector.x = Screen.width * (0.5f - vector.x);
                                vector.z = 0f;
                            }
                            else
                                continue;

                            vector = monsterTransform.position - campos;
                            vector.y += 1.2f;
                            float curdis = vector.magnitude;
                            var hits = Physics.RaycastAll(new Ray(campos, vector), curdis);
                            bool visible = true;

                            foreach (RaycastHit raycastHit in hits)
                            {
                                if (raycastHit.collider.gameObject.layer == 0 || raycastHit.collider.gameObject.layer == 30 || raycastHit.collider.gameObject.layer == 31)
                                {
                                    visible = false;
                                    break;
                                }
                            }

                            if (visible && curdis < neardis)
                            {
                                neardis = curdis;
                                nearmons = monsterTransform;
                            }

                        }
                        // 修改玩家摄像机角度并且瞄准
                        if (nearmons != null)
                        {
                            Vector3 objpos = default;
                            objpos.x = HeroCameraManager.HeroObj.gameTrans.position.x;
                            objpos.y = nearmons.position.y + 0.2f;
                            objpos.z = HeroCameraManager.HeroObj.gameTrans.position.z;
                            Vector3 forward = nearmons.position - objpos;
                            forward.y += 0.12f;
                            Quaternion rotation = Quaternion.LookRotation(forward);
                            HeroCameraManager.HeroObj.gameTrans.rotation = rotation;
                            forward = nearmons.position - campos;
                            forward.y += 0.12f;
                            Quaternion rotation2 = Quaternion.LookRotation(forward);
                            CameraManager.MainCamera.rotation = rotation2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Msg("异常:" + ex.ToString());
            }
        }

        public override void OnGUI()
        {
            if (showUI)
            {
                GUILayout.BeginArea(new Rect(0f, 300f, 150f, 200f));
                GUILayout.Label("重新初始化(" + needinitKey.ToString() + ")", null);
                GUILayout.Label("辅助瞄准(" + autoaimKey.ToString() + ")：" + (autoaim ? "开" : "关"), null);
                GUILayout.Label("武器增强(" + weaponEnhanceKey.ToString() + ")：" + (weaponEnhance ? "开" : "关"), null);
                GUILayout.Label("玩家增强(" + playerEnhanceKey.ToString() + ")：" + (playerEnhance ? "开" : "关"), null);
                GUILayout.Label("透视开关(" + shownpcKey.ToString() + ")：" + (shownpc ? "开" : "关"), null);
                GUILayout.EndArea();
            }
            if (shownpc)
            {
                try
                {
                    foreach (KeyValuePair<int, NewPlayerObject> keyValuePair in NewPlayerManager.PlayerDict)
                    {
                        NewPlayerObject value = keyValuePair.Value;
                        if (!(value.centerPointTrans == null) && ShowObject(value))
                        {
                            Vector3 vector = CameraManager.MainCameraCom.WorldToScreenPoint(value.centerPointTrans.transform.position);
                            if (vector.z > 0f)
                            {
                                string str = Vector3.Distance(HeroMoveManager.HeroObj.centerPointTrans.position, value.centerPointTrans.position).ToString("0.0");
                                GUI.Label(new Rect(vector.x, Screen.height - vector.y, 800f, 50f), FightTypeToString(value) + "(" + str + "m)");
                            }
                        }
                    }
                }
                catch
                {
                    MelonLogger.Msg("OnGUIbug");
                }
            }
        }

        public bool ShowObject(NewPlayerObject obj)
        {
            if (obj.FightType != ServerDefine.FightType.NWARRIOR_DROP_BULLET || obj.FightType != ServerDefine.FightType.NWARRIOR_DROP_CASH)
            {
                switch (obj.FightType)
                {
                    case ServerDefine.FightType.NWARRIOR_DROP_EQUIP: return true;

                    case ServerDefine.FightType.WARRIOR_OBSTACLE_NORMAL:
                        if (obj.Shape == 4406 || obj.Shape == 4419 || obj.Shape == 4427)
                            return true;
                        break;
                    case ServerDefine.FightType.NWARRIOR_NPC_TRANSFER:
                        if (obj.Shape == 4016 || obj.Shape == 4009 || obj.Shape == 4019)
                            return true;
                        break;
                    case ServerDefine.FightType.NWARRIOR_DROP_RELIC:
                    case ServerDefine.FightType.NWARRIOR_NPC_SMITH:
                    case ServerDefine.FightType.NWARRIOR_NPC_SHOP:
                    case ServerDefine.FightType.NWARRIOR_NPC_EVENT:
                    case ServerDefine.FightType.NWARRIOR_NPC_REFRESH:
                    case ServerDefine.FightType.NWARRIOR_NPC_ITEMBOX:
                    case ServerDefine.FightType.NWARRIOR_NPC_GSCASHSHOP:
                        return true;
                    default:
                        return false;
                }
            }

            return false;
        }
        public string FightTypeToString(NewPlayerObject obj)
        {
            switch (obj.FightType)
            {
                case ServerDefine.FightType.NWARRIOR_DROP_EQUIP:
                    return DataMgr.GetWeaponData(obj.Shape).Name + " +" + obj.DropOPCom.WeaponInfo.SIProp.Grade.ToString();
                case ServerDefine.FightType.NWARRIOR_DROP_RELIC:
                    return DataMgr.GetRelicData(obj.DropOPCom.RelicSid).Name;
                case ServerDefine.FightType.NWARRIOR_NPC_SMITH:
                    return "工匠";
                case ServerDefine.FightType.NWARRIOR_NPC_SHOP:
                    return "商人";
                case ServerDefine.FightType.NWARRIOR_NPC_EVENT:
                case ServerDefine.FightType.NWARRIOR_NPC_REFRESH:
                    return "事件宝箱";
                case ServerDefine.FightType.NWARRIOR_NPC_ITEMBOX:
                    return "奖励宝箱";
                case ServerDefine.FightType.WARRIOR_OBSTACLE_NORMAL:
                case ServerDefine.FightType.NWARRIOR_NPC_TRANSFER:
                    return "秘境";
                case ServerDefine.FightType.NWARRIOR_NPC_GSCASHSHOP:
                    return "奇货商";
                default:
                    return obj.Shape.ToString();
            }
        }
    }
}
