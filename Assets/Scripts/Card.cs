using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    Image m_icon;
    Image m_suit_image;

    TextMeshProUGUI m_power_text;

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

    private void Update()
    {
        LoadText();
        LoadSuit();
    }

    void Awake()
    {
        m_image = gameObject.transform.Find("Icon").GetComponent<Image>();

        m_icon = gameObject.transform.Find("Icon").GetComponent<Image>();

        m_suit_image = gameObject.transform.Find("Suit").GetComponent<Image>();

        m_power_text = gameObject.transform.Find("PowerScore").GetComponent<TextMeshProUGUI>();


        m_backSprite = Resources.Load<Sprite>("CardIcons/spirit");

        LoadImage();    
    }

    void LoadSuit()
    {
        if (!m_isFrontShowing)
        {
            m_suit_image.sprite = Resources.Load<Sprite>("Suits/unknown");
        }
        else
        {
            //clubs
            if (m_suit == Suit.club)
            {
                m_suit_image.sprite = Resources.Load<Sprite>("Suits/club");
            }
            //hearts
            if (m_suit == Suit.heart)
            {
                m_suit_image.sprite = Resources.Load<Sprite>("Suits/heart");
            }
            //spades
            if (m_suit == Suit.spade)
            {
                m_suit_image.sprite = Resources.Load<Sprite>("Suits/spade");
            }
            //diamonds
            if (m_suit == Suit.diamond)
            {
                m_suit_image.sprite = Resources.Load<Sprite>("Suits/diamond");
            }
        }

    }

    void LoadText()
    {
        if (!m_isFrontShowing)
        {
            m_power_text.text = "?";
        }
        else
        {
            m_power_text.text = m_rank.ToString();
        }
    }

    void LoadImage()
    {
        string imageName = "";
        if (m_suit != Suit.none)
        {
            imageName = "PlayingCards/" + s_suitName[(int)m_suit] + string.Format("{0:00}", m_rank);
        }

        //Icon
        string iconName = "";

        //spy
        if (m_rank == 2
            || m_rank == 3
            || m_rank == 4)
        {
            iconName = "CardIcons/SpyIconScaled";
        }
        else if (m_rank >= 10) {
            iconName = "CardIcons/KingIconScaled";
        } else
        {
            iconName = "CardIcons/KnightIconScaled";
        }

        Debug.Log("Loading image " + imageName);

        m_frontSprite = Resources.Load<Sprite>(iconName);
        if (m_isFrontShowing)
            m_icon.sprite = m_frontSprite;
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
            m_icon.sprite = m_frontSprite;
        }
        else
        {
            m_icon.sprite = m_backSprite;
        }
    }

    public void OnClick()
    {
            // Turn the card over
            SetFaceUp(!m_isFrontShowing);

    }
}
