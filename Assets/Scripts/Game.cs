using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Game : MonoBehaviour
{
    //Scene
    public string MainMenuSceneName;

    //Game Management
    public static int overallP1Score = 0;
    public static int overallP2Score = 0;
    public TextMeshProUGUI overallScore;

    public static bool isPlayer1Turn = true;
    public static int turnNumber = 1;
    public static int movesLeft = 2;
    public TextMeshProUGUI movesLeftText;

    //Lane Management
    public static Card selectedCard;
    //P1
    public Lane P1Lane1;
    public Lane P1Lane2;
    public Lane P1Lane3;

    public TextMeshProUGUI P1Lane1Score;
    public TextMeshProUGUI P1Lane2Score;
    public TextMeshProUGUI P1Lane3Score;

    public GameObject Player1Cover;

    //P2
    public Lane P2Lane1;
    public Lane P2Lane2;
    public Lane P2Lane3;

    public TextMeshProUGUI P2Lane1Score;
    public TextMeshProUGUI P2Lane2Score;
    public TextMeshProUGUI P2Lane3Score;

    public GameObject Player2Cover;

    //deck
    public Deck m_deck;

    //discard
    public Discard m_discardPile;

    //WinScreen
    public GameObject winScreen;
    public TextMeshProUGUI winText;

    //Buttons
    public GameObject m_SummonButton;
    public GameObject m_TradeButton;

    public GameObject m_playAgain;

    int m_scorePlayer = 0;
    int m_scoreDealer = 0;

    bool winCounted = false;
    private void Start()
    {
        StartCoroutine(DelayedDeal());
    }

    private void Update()
    {
        //Player Covers
        if (isPlayer1Turn)
        {
            Player2Cover.SetActive(true);
            Player1Cover.SetActive(false);
        } else
        {
            Player1Cover.SetActive(true);
            Player2Cover.SetActive(false);
        }
        //Movesleft
        movesLeftText.text = movesLeft.ToString();

        if (movesLeft <= 0)
        {
            m_SummonButton.gameObject.GetComponent<Button>().interactable = false;
            m_TradeButton.gameObject.GetComponent<Button>().interactable = false;
        } else
        {
            m_SummonButton.gameObject.GetComponent<Button>().interactable = true;
            m_TradeButton.gameObject.GetComponent<Button>().interactable = true;
        }

        //Overall Score
        overallScore.text = "P1: " + overallP1Score + " - " + "P2: " + overallP2Score;
        
    }

    private void LateUpdate()
    {
        if (turnNumber > 2)
        {
            CheckGameEnd();
        }
        
    }

    public void EndTurn()
    {
        if (isPlayer1Turn)
        {
            isPlayer1Turn = false;
        } else
        {
            isPlayer1Turn = true;
        }
        movesLeft = 2;
        turnNumber++;
    }

    public void UseMove()
    {
        movesLeft--;
    }

    public void CheckGameEnd()
    {
        //p1 has all cards showing
        if (P1Lane1.m_cards[0].m_isFrontShowing && P1Lane1.m_cards[1].m_isFrontShowing
            && P1Lane2.m_cards[0].m_isFrontShowing && P1Lane2.m_cards[1].m_isFrontShowing
            && P1Lane3.m_cards[0].m_isFrontShowing && P1Lane3.m_cards[1].m_isFrontShowing)
        {
            P2Lane1.m_cards[0].SetFaceUp(true);
            P2Lane1.m_cards[1].SetFaceUp(true);
            P2Lane2.m_cards[0].SetFaceUp(true);
            P2Lane2.m_cards[1].SetFaceUp(true);
            P2Lane3.m_cards[0].SetFaceUp(true);
            P2Lane3.m_cards[1].SetFaceUp(true);

            ScoreEachLane();
            Debug.Log("Game Over");
        }

        //p2 has all cards showing
        if (P2Lane1.m_cards[0].m_isFrontShowing && P2Lane1.m_cards[1].m_isFrontShowing
            && P2Lane2.m_cards[0].m_isFrontShowing && P2Lane2.m_cards[1].m_isFrontShowing
            && P2Lane3.m_cards[0].m_isFrontShowing && P2Lane3.m_cards[1].m_isFrontShowing)
        {
            P1Lane1.m_cards[0].SetFaceUp(true);
            P1Lane1.m_cards[1].SetFaceUp(true);
            P1Lane2.m_cards[0].SetFaceUp(true);
            P1Lane2.m_cards[1].SetFaceUp(true);
            P1Lane3.m_cards[0].SetFaceUp(true);
            P1Lane3.m_cards[1].SetFaceUp(true);

            ScoreEachLane();
            Debug.Log("Game Over");
        }

    }
    public void ScoreEachLane()
    {
        int p1Score = 0;
        int p2Score = 0;

        //Lane1
        if (P1Lane1.Score > P2Lane1.Score)
        {
            p1Score++;
        } else if (P1Lane1.Score < P2Lane1.Score)
        {
            p2Score++;
        }
        //Lane2
        if (P1Lane2.Score > P2Lane2.Score)
        {
            p1Score++;
        }
        else if (P1Lane2.Score < P2Lane2.Score)
        {
            p2Score++;
        }
        //Lane3
        if (P1Lane3.Score > P2Lane3.Score)
        {
            p1Score++;
        }
        else if (P1Lane3.Score < P2Lane3.Score)
        {
            p2Score++;
        }

        if (p2Score > p1Score)
        {
            winText.text = "Player 2 Wins!";
            if(!winCounted)
            {
                overallP2Score++;
                winCounted = true;
            }
        } else
        {
            winText.text = "Player 1 Wins!";
            if (!winCounted)
            {
                overallP1Score++;
                winCounted = true;
            }
            
        }

        winScreen.SetActive(true);
    }

    IEnumerator DelayedDeal()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Deal();
    }

    void Deal()
    {
        // deal 2 cards to the player in each lane
        P1Lane1.AddCard(m_deck.GetCard(), false);
        P1Lane1.AddCard(m_deck.GetCard(), false);

        P1Lane2.AddCard(m_deck.GetCard(), false);
        P1Lane2.AddCard(m_deck.GetCard(), false);

        P1Lane3.AddCard(m_deck.GetCard(), false);
        P1Lane3.AddCard(m_deck.GetCard(), false);


        // deal 2 cards to the dealer
        P2Lane1.AddCard(m_deck.GetCard(), false);
        P2Lane1.AddCard(m_deck.GetCard(), false);

        P2Lane2.AddCard(m_deck.GetCard(), false);
        P2Lane2.AddCard(m_deck.GetCard(), false);

        P2Lane3.AddCard(m_deck.GetCard(), false);
        P2Lane3.AddCard(m_deck.GetCard(), false);

    }

    public void SetSelectedCard(Card card)
    {
        selectedCard = card;
    }

    //player functions
    public void TradeCardWithDiscard()
    {

        //putting discard card into selected slot
        selectedCard.GetComponentInParent<Lane>().AddCardAtSlot(m_discardPile.GetCard(), selectedCard.laneIndex);
        selectedCard.GetComponentInParent<Lane>().RemoveCard(selectedCard);

        //putting selected card in discard pile
        m_discardPile.AddCard(selectedCard, true);

    }

    public void PlayAgain()
    {
        winScreen.SetActive(false);
        P1Lane1.Clear();
        P1Lane2.Clear();
        P1Lane3.Clear();
        P2Lane1.Clear();
        P2Lane2.Clear();
        P2Lane3.Clear();
        winCounted = false;
        //P2Lane1Score.transform.parent.gameObject.SetActive(false);
        m_deck.Reset();
        m_discardPile.Reset();
        //m_hitButton.SetActive(true);
        //m_stayButton.SetActive(true);
        movesLeft = 2;
        isPlayer1Turn = true;
        selectedCard = null;

        Deal();
        m_deck.DiscardTopCard();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }
}
