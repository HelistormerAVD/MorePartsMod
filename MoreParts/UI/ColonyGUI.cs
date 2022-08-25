﻿using MorePartsMod.Buildings;
using MorePartsMod.Managers;
using SFS.Input;
using SFS.UI.ModGUI;
using UnityEngine;
using UnityEngine.UI;
using static MorePartsMod.Buildings.ColonyComponent;

namespace MorePartsMod.UI
{
    class ColonyGUI : Screen_Base
    {
        public override bool PauseWhileOpen => false;
        public ColonyComponent Colony {set{
                this._colony = value;
            } 
        }

        private Window _holder;
        private string _name;
        private ColonyComponent _colony;

        public override void OnClose()
        {
            GameObject.Destroy(this._holder.gameObject);
            if (this._name != this._colony.data.name)
            {
                this._colony.data.name = this._name;
               
                ColonyManager.main.SaveWoldInfo();
            }
        }

        public override void OnOpen()
        {
            this._name = this._colony.data.name;
            this._holder = Builder.CreateWindow(this.transform.gameObject, 500, 700, 0, 0, false, 1, "Colony Menu", Builder.Style.Blue);
            this._holder.CreateLayoutGroup(LayoutType.Vertical).spacing = 20f;
            this._holder.CreateLayoutGroup(LayoutType.Vertical).DisableChildControl();
            this._holder.CreateLayoutGroup(LayoutType.Vertical).childAlignment = TextAnchor.UpperCenter;
            this.generateUI();
        }

        public override void ProcessInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape)){
                ScreenManager.main.CloseCurrent();
            }
        }

        private void onChangeColonyName(string value)
        {
            this._name = value;
        }

        private void generateUI()
        {
            Builder.CreateLabel(this._holder.ChildrenHolder, 480, 35, 0, 0, "Information");
            Builder.CreateTextInput(this._holder.ChildrenHolder, 480, 50, 0, 0, this._colony.data.name,this.onChangeColonyName);

            Builder.CreateLabel(this._holder.ChildrenHolder, 480, 35, 0, 0, "Buildings");

            foreach(ColonyBuildingData building in this._colony.data.buildings)
            {
                if (building.state)
                {
                    continue;
                }
                Builder.CreateButton(this._holder.ChildrenHolder, 480, 60, 0, 0, () => this._colony.Build(building.name), "Build " + building.name, Builder.Style.Blue);
            }

            Builder.CreateButton(this._holder.ChildrenHolder, 480, 60, 40, 0, () => ScreenManager.main.CloseCurrent(), "Close", Builder.Style.Blue);
        }


    }
}
//