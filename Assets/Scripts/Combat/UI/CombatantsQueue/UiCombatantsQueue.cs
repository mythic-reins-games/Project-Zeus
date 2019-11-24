using System.Collections.Generic;
using UnityEngine;

namespace Battle.Ui.CombatantsQueue
{
    public class UiCombatantsQueue : MonoBehaviour
    {
        IReadOnlyList<CombatController> _players;
        [SerializeField] UiCombatantWindow[] windows;

        void Awake() => Subscribe();
        void OnDestroy() => UnSubscribe();

        void OnSetUpPlayers(IReadOnlyList<CombatController> players) => _players = players;

        void OnTurnBegin(int index) => UpdateView(index);

        void UpdateView(int index)
        {
            var count = windows.Length;
            var maxPlayers = _players.Count;
            for (var i = 0; i < count; i++)
            {
                var position = (i + index) % maxPlayers;
                var window = windows[i];
                var player = _players[position];
                if (player != null)
                {
                    var creature = player.GetComponent<CreatureMechanics>();
                    window.UpdateData(creature);
                    window.UpdatePosition(i);
                }
                else
                {
                    window.UpdateDead();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        #region Event System

        // TODO:
        // Verify which event system will be integrated in the TurnManager.
        // Then adapt the next implementation to follow the standard
        // event system that will be kept int the game.

        void Subscribe()
        {
            TurnManager.OnTurnBegin += OnTurnBegin;
            TurnManager.OnSetUpPlayers += OnSetUpPlayers;
        }

        void UnSubscribe()
        {
            TurnManager.OnTurnBegin -= OnTurnBegin;
            TurnManager.OnSetUpPlayers -= OnSetUpPlayers;
        }

        #endregion
    }
}