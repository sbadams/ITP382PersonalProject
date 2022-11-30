using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lane : MonoBehaviour
{
    public GameObject CardPos1;
    public GameObject CardPos2;
    public bool isPlayer1 = true;
    public int Score;
    private TextMeshProUGUI scoreText;
    public bool suitBonus = false;
    public List<Card> m_cards = new List<Card>();

    private void Update()
    {

        suitBonus = CheckSuitBonus();
        Score = CalculateLaneScore();
        scoreText.text = Score.ToString();
    }
    private void Awake()
    {
        scoreText = gameObject.transform.Find("Score").GetComponent<TextMeshProUGUI>();
    }
    public void AddCard(Card card, bool isFaceUp)
    {
        card.transform.SetParent(transform, false);
        if (m_cards.Count > 0)
        {
            card.transform.localPosition = CardPos2.transform.localPosition;
            card.laneIndex = 1;
        }
        else
        {
            card.transform.localPosition = CardPos1.transform.localPosition;
            card.laneIndex = 0;
        }
        //rotation
        if (!isPlayer1) {
            card.transform.localEulerAngles = new Vector3(card.transform.localEulerAngles.x, card.transform.localEulerAngles.y, -180.0f);
        }
        card.SetFaceUp(isFaceUp);
        m_cards.Add(card);
    }

    public bool CheckSuitBonus()
    {
        if (m_cards.Count > 0)
        {
            if (m_cards[0].m_isFrontShowing && m_cards[1].m_isFrontShowing)
            {
                if (m_cards[0].m_suit == m_cards[1].m_suit)
                {
                    return true;
                }
            }
        }

        return false;
    }
    public void AddCardAtSlot(Card card, int index)
    {
        card.transform.SetParent(transform, false);
        if (index == 0)
        {
            card.transform.localPosition = CardPos1.transform.localPosition;
        } else
        {
            card.transform.localPosition = CardPos2.transform.localPosition;
        }
        if (!isPlayer1)
        {
            card.transform.localEulerAngles = new Vector3(card.transform.localEulerAngles.x, card.transform.localEulerAngles.y, -180.0f);
        }
        else
        {
            card.transform.localEulerAngles = new Vector3(card.transform.localEulerAngles.x, card.transform.localEulerAngles.y, 0.0f);
        }
        card.SetFaceUp(true);
        m_cards.Add(card);
    }
    public int CalculateLaneScore()
    {
        int score = 0;
        int numAces = 0;

        // TODO calculate the score
        foreach (Card card in m_cards)
        {
            if (card.m_rank == 1)
            {
                //ace
                numAces++;
            }

            score += Card.s_value[card.m_rank];
        }

        //aces
        for (int i = 0; i < numAces; i++)
        {
            if (score + 10 <= 21)
            {
                score += 10;
            }
        }

        //suit bonus
        if (suitBonus)
        {
            score += 5;
            m_cards[0].GetComponent<Animator>().SetTrigger("Attack");
            m_cards[1].GetComponent<Animator>().SetTrigger("Attack");
        }

        return score;
    }

    public int NumCards()
    {
        return m_cards.Count;
    }

    public void RemoveCard(Card card) {
        Debug.Log("Removing a card from the slot");
        //m_cards[index].gameObject.SetActive(false);
        m_cards.Remove(card);
    }

    public void RevealAll()
    {
        foreach (Card card in m_cards)
        {
            card.SetFaceUp(true);
        }
    }

    public void Clear()
    {
        foreach (Card card in m_cards)
        {
            Destroy(card.gameObject);
        }
        m_cards.Clear();
    }
}
