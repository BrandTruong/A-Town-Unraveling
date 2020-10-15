using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverButtons : MonoBehaviour
{
    /* Endings 1 and 2 should occur once 6 AM rolls around and the game detects how
     * many vampires were killed (regardless of how many innocent people died).
     * Ending 3 should occur right after the last vampire is killed, checking that
     * the other two are also dead.
     * Ending 4 should happen when the parents catch the player killing Vampire 2
     * (Grandma) with the wrong weapon, or when the barkeeper catches the player
     * killing Vampire 3 (Spice Vendor) with the wrong weapon.*/

    public TimeManager currentTimeInterval;
    public GameObject end1UI;
    public GameObject end2UI;
    public GameObject end3UI;
    public GameObject CaughtUI;

    public Text endText;
    string morningAfter1;
    string morningAfter2;

    public int curInterval;

    void Start()
    {
        end1UI.SetActive(false);
        end2UI.SetActive(false);
        end3UI.SetActive(false);
        CaughtUI.SetActive(false);
    }

    void Update()
    {
        //use curInterval below to detect when the time has reached 6am (which would be interval 48)
        //also here change the name of game object to find to whatever object has the time manager script attached to it
        curInterval = currentTimeInterval.getInterval();

        //if player was caught, CaughtUI.SetActive(true); Caught();
        if (StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_STAKE) || StoryManager.instance.CheckKey(Conditional.GRANNY_PLAYER_CAUGHT) || StoryManager.instance.CheckKey(Conditional.INNOCENT_KILLED))
        {
            CaughtUI.SetActive(true); 
            Caught();
        }
        //else if player kills third vampire and other two were also killed, end3UI.SetActive(true); Success();
        else if( (StoryManager.instance.CheckKey(Conditional.CHESSMASTER_BLADE) || StoryManager.instance.CheckKey(Conditional.CHESSMASTER_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.CHESSMASTER_STAKE)) &&
                 (StoryManager.instance.CheckKey(Conditional.GRANNY_BLADE) || StoryManager.instance.CheckKey(Conditional.GRANNY_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.GRANNY_STAKE)) &&
                 (StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_BLADE) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_STAKE))
            )
        {
            end3UI.SetActive(true);
            Success();
        }
        //else if player gets to end of day and killed at least 1 vampire, end2UI.SetActive(true); Failure2();
        else if ((StoryManager.instance.CheckKey(Conditional.CHESSMASTER_BLADE) || StoryManager.instance.CheckKey(Conditional.CHESSMASTER_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.CHESSMASTER_STAKE)) ||
                 (StoryManager.instance.CheckKey(Conditional.GRANNY_BLADE) || StoryManager.instance.CheckKey(Conditional.GRANNY_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.GRANNY_STAKE)) ||
                 (StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_BLADE) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_STAKE))
            )
        {
            end2UI.SetActive(true);
            Failure2();
        }
        //else if player gets to end of day and no vampires are killed, end1UI.SetActive(true); Failure1();
        else if ( curInterval >= 48)
        {
            end1UI.SetActive(true);
            Failure1();
        }
    }

    void Failure1()
    {
        Time.timeScale = 0f;
        //if the emo kid was killed, morningAfter1 = "In the morning, the corpses of a peculiar boy, a little girl, and the local barkeeper were found with gruesome wounds.";
        if(StoryManager.instance.CheckKey(Conditional.PECULIARBOY_BLADE) || StoryManager.instance.CheckKey(Conditional.PECULIARBOY_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.PECULIARBOY_STAKE))
        {
            morningAfter1 = "In the morning, the corpses of a peculiar boy, a little girl, and the local barkeeper were found with gruesome wounds.";
        }
        //else the emo kid was not killed, morningAfter1 = "In the morning, the corpses of a little girl and the local barkeeper were found with gruesome wounds.";
        else
        {
            morningAfter1 = "In the morning, the corpses of a little girl and the local barkeeper were found with gruesome wounds.";
        }
            endText.text = ("Failure:" + "\n\n" + "Despite your best efforts, you did not accomplish your goal. In fact, you did not kill a single vampire. "+ morningAfter1 + "\n\n"+"You may have failed, but you know more than you did before. On your honor as a detective, it's time to try again... until you succeed!").ToString();
    }

    void Failure2()
    {
        Time.timeScale = 0f;
        //if vampire 1 was killed
        if ((StoryManager.instance.CheckKey(Conditional.CHESSMASTER_BLADE) || StoryManager.instance.CheckKey(Conditional.CHESSMASTER_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.CHESSMASTER_STAKE)))
        {
            if((StoryManager.instance.CheckKey(Conditional.GRANNY_BLADE) || StoryManager.instance.CheckKey(Conditional.GRANNY_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.GRANNY_STAKE)))
            {
                morningAfter2 = "In the morning, the corpses of a chess champion, an old lady, and the local barkeeper were found with gruesome wounds.";
            }
            else if((StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_BLADE) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_STAKE)))
            {
                morningAfter2 = "In the morning, the corpses of a chess champion, a little girl, and a spice vendor were found with gruesome wounds.";
            }
            else
            {
                morningAfter2 = "In the morning, the corpse of a chess champion was found with gruesome wounds.";
            }
        }
            //if vampire 2 was killed, morningAfter2="In the morning, the corpses of a chess champion, an old lady, and the local barkeeper were found with gruesome wounds.";
            //else if vampire 3 was killled, morningAfter2="In the morning, the corpses of a chess champion, a little girl, and a spice vendor were found with gruesome wounds.";
            //else neither were killed, morningAfter2="In the morning, the corpse of a chess champion was found with gruesome wounds.";

        //else if emo kid was killed
        else if((StoryManager.instance.CheckKey(Conditional.PECULIARBOY_BLADE) || StoryManager.instance.CheckKey(Conditional.PECULIARBOY_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.PECULIARBOY_STAKE)))
        {
            if((StoryManager.instance.CheckKey(Conditional.GRANNY_BLADE) || StoryManager.instance.CheckKey(Conditional.GRANNY_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.GRANNY_STAKE)) &&
                (StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_BLADE) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_STAKE))
                )
            {
                morningAfter2 = "In the morning, the corpses of a peculiar boy, an old lady, and a spice vendor were found with gruesome wounds.";
            }
            else if((StoryManager.instance.CheckKey(Conditional.GRANNY_BLADE) || StoryManager.instance.CheckKey(Conditional.GRANNY_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.GRANNY_STAKE)))
            {
                morningAfter2 = "In the morning, the corpses of a peculiar boy, an old lady, and the local barkeeper were found with gruesome wounds.";
            }
            else if((StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_BLADE) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_STAKE)))
            {
                morningAfter2 = "In the morning, the corpses of a peculiar boy, a little girl, and a spice vendor were found with gruesome wounds.";
            }
        }
        //if both vampire 2 and 3 were killed, morningAfter2="In the morning, the corpses of a peculiar boy, an old lady, and a spice vendor were found with gruesome wounds.";
        //else if only vampire 2 was killed, morningAfter2="In the morning, the corpses of a peculiar boy, an old lady, and the local barkeeper were found with gruesome wounds.";
        //else only vampire 3 was killed, morningAfter2="In the morning, the corpses of a peculiar boy, a little girl, and a spice vendor were found with gruesome wounds.";

        else
        {
            if ((StoryManager.instance.CheckKey(Conditional.GRANNY_BLADE) || StoryManager.instance.CheckKey(Conditional.GRANNY_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.GRANNY_STAKE)) &&
                (StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_BLADE) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_STAKE))
                )
            {
                morningAfter2 = "In the morning, the corpses of an old lady and a spice vendor were found with gruesome wounds.";
            }
            else if ((StoryManager.instance.CheckKey(Conditional.GRANNY_BLADE) || StoryManager.instance.CheckKey(Conditional.GRANNY_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.GRANNY_STAKE)))
            {
                morningAfter2 = "In the morning, the corpse of an old lady was found with gruesome wounds.";
            }
            else if ((StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_BLADE) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_HOLYWATER) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_STAKE)))
            {
                morningAfter2 = "In the morning, the corpse of a spice vendor was found with gruesome wounds.";
            }
        }
        //else neither vampire 1 nor emo kid were killed
            //if both vampire 2 and 3 were killed, morningAfter2="In the morning, the corpses of an old lady and a spice vendor were found with gruesome wounds.";
            //else if only vampire 2 was killed, morningAfter2="In the morning, the corpse of an old lady was found with gruesome wounds.";
            //else only vampire 3 was killed, morningAfter2="In the morning, the corpse of a spice vendor was found with gruesome wounds.";

        endText.text = ("Failure:" + "\n\n" + "Despite your best efforts, you did not accomplish your goal. You managed to kill at least one vampire. " + morningAfter2 + "\n\n" + "You may have failed, but you know more than you did before. On your honor as a detective, it's time to try again... until you succeed!").ToString();
    }

    void Caught()
    {
        Time.timeScale = 0f;
        if (StoryManager.instance.CheckKey(Conditional.INNOCENT_KILLED))
        {
            endText.text = ("Failure: an innocent has been killed...");
        }

        //if caught by parents
        if (StoryManager.instance.CheckKey(Conditional.GRANNY_PLAYER_CAUGHT))
        {
            endText.text = ("Failure:" + "\n\n" + "\"Aaahhhhh!\" the granny screams." + "\n\n" + "\"What was that?!\" The parents rush into the room and see what you've done. \"Oh my God! Murder! Murder!\"" + "\n\n" + "There's no way out of it. They're convinced that you're a cold-blooded killer and will call the authorities. The only thing you can do now is try again.").ToString();
        }

        //if caught by barkeeper
        if (StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_STAKE) || StoryManager.instance.CheckKey(Conditional.SPICEVENDOR_HOLYWATER))
        {
            endText.text = ("Failure:" + "\n\n" + "\"Auuuughhh!\" the spice vendor screams." + "\n\n" + "\"What's going on?!\" The barkeeper rushes into the room and sees what you've done. \"MURDERER!\"" + "\n\n" + "There's no way out of it. He's convinced that you're a cold-blooded killer and will call the authorities. The only thing you can do now is try again.").ToString();
        }

    }

    void Success()
    {
        Time.timeScale = 0f;
        endText.text = ("Success:" + "\n\n" + "You take a sigh of relief. All three vampires are dead. Any innocent deaths that would have occurred by their hands have been avoided. Even if no one knows, you have maintained your honor and fulfilled your duty to keep the town safe." + "\n\n" + "Now, it is time to move forward in life, until danger arises again...");
    }

    public void TryAgainButton()
    {
        //reset variables
        //load up first scene
        SceneManager.LoadScene("Town Proto");
    }

    public void EndGameButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
