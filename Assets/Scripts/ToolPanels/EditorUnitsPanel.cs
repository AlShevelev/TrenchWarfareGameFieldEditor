using System;
using System.Linq;
using TrenchWarfare.Domain.Enums;
using TrenchWarfare.ToolPanels.State;
using TrenchWarfare.Utility;
using UnityEngine.UI;

namespace TrenchWarfare.ToolPanels {
    public class EditorUnitsPanel : EditorToolPanelBase {
        public EditorState state;

        private const int FATIGUE_GRADE = 100;

        void Start() {
            var sourceUnit = state.UnitInfo;

            InitDropdown("Type", (int)sourceUnit.Type);
            InitNationDropdown();
            InitDropdown("Experience Rank", (int)sourceUnit.ExperienceRank);
            InitDropdown("Boost 1", (int)sourceUnit.Boost1);
            InitDropdown("Boost 2", (int)sourceUnit.Boost2);
            InitDropdown("Boost 3", (int)sourceUnit.Boost3);

            InitSliders();
            InitBattlesSlider();
        }

        public void SetType(int value) {
            state.UnitInfo = state.UnitInfo.Copy(i => { i.Type = (UnitType)value; return i;});

            InitSliders();
        }

        public void SetNation(int value) {
            state.UnitInfo = state.UnitInfo.Copy(i =>
            {
                i.Nation = state.Nations.GetByIndex(value);
                return i;
            });
        }

        public void SetExperienceRank(int value) {
            state.UnitInfo = state.UnitInfo.Copy(i => {
                    i.ExperienceRank = (UnitExperienceRank)value;
                    return i;
                }
            );

            InitBattlesSlider();
        }

        public void SetFatigue(float value) {
            state.UnitInfo = state.UnitInfo.Copy(i => {
                    i.Fatigue = value / FATIGUE_GRADE;
                    return i;
                }
            );
        }

       public void SetHealth(float value) {
            state.UnitInfo = state.UnitInfo.Copy(i => {
                    i.Health = value;
                    return i;
                }
            );

            UpdateHealthLabel();
        }

       public void SetMovementPoints(float value) {
            state.UnitInfo = state.UnitInfo.Copy(i => {
                    i.MovementPoints = value;
                    return i;
                }
            );

            UpdateMovementPointsLabel();
        }

       public void SetBattles(float value) {
            state.UnitInfo = state.UnitInfo.Copy(i => {
                    i.TookPartInBattles = (int)value;
                    return i;
                }
            );

            UpdateBattlesLabel();
        }

        public void SetBooster1(int value) {
            state.UnitInfo = state.UnitInfo.Copy(i => {
                    var boostValue = (UnitBoost)value;

                    if (boostValue != UnitBoost.None) {
                        if (i.Boost2  == boostValue) {
                            i.Boost2 = UnitBoost.None;
                            InitDropdown("Boost 2", (int)i.Boost2);
                        }

                        if (i.Boost3  == boostValue) {
                            i.Boost3 = UnitBoost.None;
                            InitDropdown("Boost 3", (int)i.Boost3);
                        }
                    }


                    i.Boost1 = boostValue;
                    return i;

                }
            );
        }

        public void SetBooster2(int value) {
            state.UnitInfo = state.UnitInfo.Copy(i => {
                    var boostValue = (UnitBoost)value;

                    if (boostValue != UnitBoost.None) {
                        if (i.Boost1  == boostValue) {
                            i.Boost1 = UnitBoost.None;
                            InitDropdown("Boost 1", (int)i.Boost1);
                        }

                        if (i.Boost3  == boostValue) {
                            i.Boost3 = UnitBoost.None;
                            InitDropdown("Boost 3", (int)i.Boost3);
                        }
                    }


                    i.Boost2 = boostValue;
                    return i;

                }
            );
        }

        public void SetBooster3(int value) {
            state.UnitInfo = state.UnitInfo.Copy(i => {
                    var boostValue = (UnitBoost)value;

                    if (boostValue != UnitBoost.None) {
                        if (i.Boost1  == boostValue) {
                            i.Boost1 = UnitBoost.None;
                            InitDropdown("Boost 1", (int)i.Boost1);
                        }

                        if (i.Boost2  == boostValue) {
                            i.Boost2 = UnitBoost.None;
                            InitDropdown("Boost 2", (int)i.Boost2);
                        }
                    }


                    i.Boost3 = boostValue;
                    return i;

                }
            );
        }

        private void InitSliders() {
            InitSlider("Fatigue", 0, 0, 1 * FATIGUE_GRADE);

            var maxHealth = state.UnitInfo.MaxHealth;
            InitSlider("Health", maxHealth, 0, maxHealth);
            UpdateHealthLabel();

            var maxMovementPoins = state.UnitInfo.MaxMovementPoints;
            InitSlider("Movement Points", maxMovementPoins, 0, maxMovementPoins);
            UpdateMovementPointsLabel();
        }

        private void InitBattlesSlider() {
            var battles = GetBattlesRange();
            InitSlider("Battles", battles.Start.Value, battles.Start.Value, battles.End.Value);
            UpdateBattlesLabel();
        }

        private Range GetBattlesRange() {
            var min = state.UnitInfo.GetBattlesForExperienceRank(state.UnitInfo.ExperienceRank);
            var max = state.UnitInfo.ExperienceRank switch {
                UnitExperienceRank.Rookies =>
                    state.UnitInfo.GetBattlesForExperienceRank(UnitExperienceRank.Fighters) - 1,
                UnitExperienceRank.Fighters =>
                    state.UnitInfo.GetBattlesForExperienceRank(UnitExperienceRank.Proficients) - 1,
                UnitExperienceRank.Proficients =>
                    state.UnitInfo.GetBattlesForExperienceRank(UnitExperienceRank.Veterans) - 1,
                UnitExperienceRank.Veterans =>
                    state.UnitInfo.GetBattlesForExperienceRank(UnitExperienceRank.Elite) - 1,
                UnitExperienceRank.Elite =>
                    state.UnitInfo.GetBattlesForExperienceRank(UnitExperienceRank.Elite) + 5,
                _ => throw new NotImplementedException()
            };

            return new Range(min, max);
        }

        private void UpdateHealthLabel() {
            InitLabel(
                "Health label",
                string.Format(
                    "Health [0, {0}, {1}]",
                    state.UnitInfo.Health,
                    state.UnitInfo.MaxHealth
                )
            );
        }

       private void UpdateMovementPointsLabel() {
            InitLabel(
                "Movement Points label",
                string.Format(
                    "Movement points [0, {0}, {1}]",
                    state.UnitInfo.MovementPoints,
                    state.UnitInfo.MaxMovementPoints
                )
            );

        }

       private void UpdateBattlesLabel() {
            var battles = GetBattlesRange();

            InitLabel(
                "Battles label",
                string.Format(
                    "Battles total [{0}, {1}, {2}]",
                    battles.Start.Value,
                    state.UnitInfo.TookPartInBattles,
                    battles.End.Value
                )
            );

        }

       protected void InitNationDropdown() {
            var dropdown = GetComponent<Dropdown>("Nation");

            dropdown.ClearOptions();
            dropdown.AddOptions(state.Nations.Select(i => i.ToString()).ToList());
            
            dropdown.value = state.Nations.IndexOf(i => i == state.UnitInfo.Nation);
        }
    }
}
