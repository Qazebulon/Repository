using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace TTT
{
    public class speechManager : MonoBehaviour
    {
        public Text text;

        KeywordRecognizer keywordRecognizer;
        Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
        TTTGame game;
        bool human_x = true;
        bool human_o = false;
        bool menu_active = true;
        bool menu_keyword_recognized = false;

        // Use this for initialization
        void Start()
        {
            game = new TTTGame(text, human_x, human_o);

            text.text = "THIS IS A TEST!!!";

            //Create keywords for keyword recognizer
            keywords.Add("one", () => { game.handleInput(0); });
            keywords.Add("two", () => { game.handleInput(1); });
            keywords.Add("three", () => { game.handleInput(2); });
            keywords.Add("four", () => { game.handleInput(3); });
            keywords.Add("five", () => { game.handleInput(4); });
            keywords.Add("six", () => { game.handleInput(5); });
            keywords.Add("seven", () => { game.handleInput(6); });
            keywords.Add("eight", () => { game.handleInput(7); });
            keywords.Add("nine", () => { game.handleInput(8); });

            keywords.Add("top left", () => { game.handleInput(0); });
            keywords.Add("top center", () => { game.handleInput(1); });
            keywords.Add("top right", () => { game.handleInput(2); });
            keywords.Add("center left", () => { game.handleInput(3); });
            keywords.Add("center center", () => { game.handleInput(4); });
            keywords.Add("center", () => { game.handleInput(4); });
            keywords.Add("center right", () => { game.handleInput(5); });
            keywords.Add("bottom left", () => { game.handleInput(6); });
            keywords.Add("bottom center", () => { game.handleInput(7); });
            keywords.Add("bottom right", () => { game.handleInput(8); });

            keywords.Add("human verses human", () => { human_x = true; human_o = true; game.resetGame(text, human_x, human_o); });
            keywords.Add("human verses computer", () => { human_x = true; human_o = false; game.resetGame(text, human_x, human_o); });
            keywords.Add("computer verses human", () => { human_x = false; human_o = true; game.resetGame(text, human_x, human_o); });
            keywords.Add("computer verses computer", () => { human_x = false; human_o = false; game.resetGame(text, human_x, human_o); });
            keywords.Add("new game", () => { game.resetGame(text, human_x, human_o); });
            keywords.Add("exit", () => { Application.Quit(); });
            keywords.Add("menu", () =>
            {
                menu_keyword_recognized = true;
                if (menu_active) { game.hideMenuText(); menu_active = false; }
                else { game.displayMenuText(); menu_active = true; }
            });

            keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
            keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;               //<<<
            keywordRecognizer.Start();
        }
        private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            System.Action keywordAction;
            // if the keyword recognized is in our dictionary, call that Action.
            if (keywords.TryGetValue(args.text, out keywordAction))                                     //<<<
            {
                keywordAction.Invoke();
                text.text += "Request Recognized\n";
            }
            if (!menu_keyword_recognized && menu_active) { game.hideMenuText(); menu_active = false; } //Cleanup after Menu state is left.
            menu_keyword_recognized = false;
        }
    }
}
