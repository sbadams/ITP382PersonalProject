using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{
    public Hand m_playerHand;
    public TextMeshProUGUI m_playerScore;
    public Hand m_dealerHand;
    public TextMeshProUGUI m_dealerScore;
    public Deck m_deck;
    public GameObject m_hitButton;
    public GameObject m_stayButton;
    public GameObject m_win;
    public GameObject m_lose;
    public GameObject m_playAgain;

    int m_scorePlayer = 0;
    int m_scoreDealer = 0;

    private void Start()
    {
        m_dealerScore.transform.parent.gameObject.SetActive(false);  // hide the dealer's score
        StartCoroutine(DelayedDeal());
    }

    IEnumerator DelayedDeal()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Deal();
    }

    void Deal()
    {
        // deal 2 cards to the player
        m_playerHand.AddCard(m_deck.GetCard(), true);
        m_playerHand.AddCard(m_deck.GetCard(), true);
        UpdatePlayerScore();

        // deal 2 cards to the dealer
        m_dealerHand.AddCard(m_deck.GetCard(), true);
        m_dealerHand.AddCard(m_deck.GetCard(), false);
        UpdateDealerScore();
    }

    public void PlayerHit()
    {
        // TODO call GetCard() to get the next card from the deck and add it to the player's hand
        // call UpdatePlayerScore() to recalculate the player's score
        // if the player busts (score > 21), call PlayerStay()
        m_playerHand.AddCard(m_deck.GetCard(), true);
        UpdatePlayerScore();
        if (m_playerHand.Score() > 21)
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
        m_dealerHand.RevealAll();
        // activate the dealer's score display
        m_dealerScore.gameObject.SetActive(true);
        // start DealerTurn() as a coroutine
        StartCoroutine(DealerTurn());
    }

    public void PlayAgain()
    {
        m_playAgain.SetActive(false);
        m_playerHand.Clear();
        m_playerScore.text = "0";
        m_dealerHand.Clear();
        m_dealerScore.transform.parent.gameObject.SetActive(false);
        m_deck.Reset();
        m_hitButton.SetActive(true);
        m_stayButton.SetActive(true);
        Deal();
    }

    void DealerHit()
    {
        m_dealerHand.AddCard(m_deck.GetCard(), true);
        UpdateDealerScore();
    }

    int UpdatePlayerScore()
    {
        int score = m_playerHand.Score();
        if (score > 21)
            m_playerScore.text = "BUST!";
        else
            m_playerScore.text = score.ToString();
        m_scorePlayer = score;
        return score;
    }

    int UpdateDealerScore()
    {
        int score = m_dealerHand.Score();
        if (score > 21)
            m_dealerScore.text = "BUST!";
        else
            m_dealerScore.text = score.ToString();
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
