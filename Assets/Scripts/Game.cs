using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    //Game Management
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

    public GameObject m_win;
    public GameObject m_lose;
    public GameObject m_playAgain;

    int m_scorePlayer = 0;
    int m_scoreDealer = 0;

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
        } else
        {
            winText.text = "Player 1 Wins!";
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

        UpdatePlayerScore();

        // deal 2 cards to the dealer
        P2Lane1.AddCard(m_deck.GetCard(), false);
        P2Lane1.AddCard(m_deck.GetCard(), false);

        P2Lane2.AddCard(m_deck.GetCard(), false);
        P2Lane2.AddCard(m_deck.GetCard(), false);

        P2Lane3.AddCard(m_deck.GetCard(), false);
        P2Lane3.AddCard(m_deck.GetCard(), false);

        UpdateDealerScore();
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

    public void PlayerHit()
    {
        // TODO call GetCard() to get the next card from the deck and add it to the player's hand
        // call UpdatePlayerScore() to recalculate the player's score
        // if the player busts (score > 21), call PlayerStay()
        P1Lane1.AddCard(m_deck.GetCard(), true);
        UpdatePlayerScore();
        if (P1Lane1.Score > 21)
        {
            //rip you lose you suck
            PlayerStay();
        }
    }

    public void PlayerStay()
    {
        // TODO deactivate the hit button
        //m_hitButton.gameObject.SetActive(false);
        //// deactivate the stay button
        //m_stayButton.gameObject.SetActive(false);
        // call RevealAll() to reveal the dealer's hand
        P2Lane1.RevealAll();
        // activate the dealer's score display
        P2Lane1Score.gameObject.SetActive(true);
        // start DealerTurn() as a coroutine
        StartCoroutine(DealerTurn());
    }

    public void PlayAgain()
    {
        m_playAgain.SetActive(false);
        P1Lane1.Clear();
        P1Lane1Score.text = "0";
        P2Lane1.Clear();
        P2Lane1Score.transform.parent.gameObject.SetActive(false);
        m_deck.Reset();
        //m_hitButton.SetActive(true);
        //m_stayButton.SetActive(true);
        Deal();
    }

    void DealerHit()
    {
        P2Lane1.AddCard(m_deck.GetCard(), true);
        UpdateDealerScore();
    }

    int UpdatePlayerScore()
    {
        int score = P1Lane1.Score;
        if (score > 21)
            P1Lane1Score.text = "BUST!";
        else
            P1Lane1Score.text = score.ToString();
        m_scorePlayer = score;
        return score;
    }

    int UpdateDealerScore()
    {
        int score = P2Lane1.Score;
        if (score > 21)
            P2Lane1Score.text = "BUST!";
        else
            P2Lane1Score.text = score.ToString();
        m_scoreDealer = score;
        return score;
    }

    Animator PlayerWins()
    {
        m_win.SetActive(true);
        Animator anim = m_win.GetComponent<Animator>();
        anim.Play("Win", -1, 0.0f);
        return anim;
    }

    Animator PlayerLoses()
    {
        m_lose.SetActive(true);
        Animator anim = m_lose.GetComponent<Animator>();
        anim.Play("Lose", -1, 0.0f);
        return anim;
    }

    IEnumerator DealerTurn()
    {
        while (m_scoreDealer < 17)
        {
            yield return new WaitForSecondsRealtime(1.0f);
            DealerHit();
        }
        Animator anim = null;
        if (m_scorePlayer > 21)
        {   // player bust
            anim = PlayerLoses();
        }
        else if (m_scoreDealer > 21)
        {   // dealer bust
            anim = PlayerWins();
        }
        else if (m_scorePlayer > m_scoreDealer)
        {   // player's score is higher
            anim = PlayerWins();
        }
        else
        {   // dealer's score is higher
            anim = PlayerLoses();
        }
        var state = anim.GetCurrentAnimatorStateInfo(0);
        float duration = state.length;
        yield return new WaitForSeconds(duration);
        m_playAgain.SetActive(true);
    }
}
