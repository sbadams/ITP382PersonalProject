using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{
    //Game Management
    public static bool isPlayer1Turn = true;
    public static int turnNumber = 1;

    //Lane Management
    public static Card selectedCard;
    //P1
    public Lane P1Lane1;
    public Lane P1Lane2;
    public Lane P1Lane3;

    public TextMeshProUGUI P1Lane1Score;
    public TextMeshProUGUI P1Lane2Score;
    public TextMeshProUGUI P1Lane3Score;

    public GameObject P1Cover;

    //P2
    public Lane P2Lane1;
    public Lane P2Lane2;
    public Lane P2Lane3;

    public TextMeshProUGUI P2Lane1Score;
    public TextMeshProUGUI P2Lane2Score;
    public TextMeshProUGUI P2Lane3Score;

    public GameObject P2Cover;

    //deck
    public Deck m_deck;

    //discard
    public Discard m_discardPile;

    //drag drop
    
    public GameObject m_hitButton;
    public GameObject m_stayButton;
    public GameObject m_win;
    public GameObject m_lose;
    public GameObject m_playAgain;

    int m_scorePlayer = 0;
    int m_scoreDealer = 0;

    private void Start()
    {
        StartCoroutine(DelayedDeal());
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
        m_hitButton.gameObject.SetActive(false);
        // deactivate the stay button
        m_stayButton.gameObject.SetActive(false);
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
        m_hitButton.SetActive(true);
        m_stayButton.SetActive(true);
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
