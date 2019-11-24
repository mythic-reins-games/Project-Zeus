using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle.Ui.CombatantsQueue
{
    public class UiCombatantWindow : MonoBehaviour
    {
        const string kSpeed = "Speed: ";
        const string kNext = "Next";
        const string kCurrent = "Current";
        const string kDead = "Dead";
        const string kPlus = " +";
        [SerializeField] Image artworkIcon;
        [SerializeField] TMP_Text textName;
        [SerializeField] TMP_Text textPosition;
        [SerializeField] TMP_Text textSpeed;

        // ... more data related to the character displayed information.

        public void UpdateData(CreatureMechanics creature)
        {
            textName.text = creature.displayName;
            textSpeed.text = kSpeed + creature.Speed;
            // ... Update other stuff

            var isAi = creature.GetComponent<EnemyController>() != null;
            textName.color = isAi ? Color.red : Color.blue;
        }

        public void UpdatePosition(int i)
        {
            switch (i)
            {
                case 0:
                    textPosition.text = kCurrent;
                    break;
                case 1:
                    textPosition.text = kNext;
                    break;
                default:
                {
                    var next = i - 1;
                    textPosition.text = string.Join(string.Empty, kNext, kPlus, next);
                    break;
                }
            }
        }

        public void UpdateDead()
        {
            textPosition.text = kDead;
            textName.text = kDead;
            textSpeed.text = kDead;
        }
    }
}