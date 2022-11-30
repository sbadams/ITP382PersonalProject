using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public enum Suit
    {
        club,
        diamond,
        heart,
        spade,
        none
    }

    public static readonly int[] s_value = new int[]
    {   //                                J   Q   K
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10
    };
    public Suit m_suit = Suit.club;
    public int m_rank = 1;  // ace=1, king=13, joker=0 or 1

    Image m_image;
    Sprite m_backSprite;
    Sprite m_frontSprite;
    bool m_isFrontShowing = false;

    public static readonly string[] s_suitName = new string[]
    {
        "Club",
        "Diamond",
        "Heart",
        "Spade",
        "Joker_"
    };

    void Awake()
    {
        m_image = GetComponent<Image>();
        m_backSprite = m_image.sprite;
        LoadImage();    
    }

    void LoadImage()
    {
        string imageName = "";
        if (m_suit != Suit.none)
        {
            imageName = "PlayingCards/" + s_suitName[(int)m_suit] + string.Format("{0:00}", m_rank);
        }
        else
        {
            if (m_rank == 0)
            {
                imageName = "PlayingCards/Joker_Color";
            }
            else
            {
                imageName = "PlayingCards/Joker_Monochrome";
            }
        }
        Debug.Log("Loading image " + imageName);
        m_frontSprite = Resources.Load<Sprite>(imageName);
        if (m_isFrontShowing)
            m_image.sprite = m_frontSprite;
    }

    public void Initialize(Suit suit, int rank)
    {
        m_suit = suit;
        m_rank = rank;
        gameObject.name = s_suitName[(int)m_suit] + string.Format("{0:00}", m_rank);
        LoadImage();
    }

    public void SetFaceUp(bool set)
    {
        m_isFrontShowing = set;
        if (m_isFrontShowing)
        {
            m_image.sprite = m_frontSprite;
        }
        else
        {
            m_image.sprite = m_backSprite;
        }
    }

    public void OnClick()
    {
        // Turn the card over
        SetFaceUp(!m_isFrontShowing);
    }
}
