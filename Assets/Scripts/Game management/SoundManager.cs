using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource; // Компонент AudioSource

    public AudioClip cardDealer; 
    public AudioClip cardPut;
    public AudioClip buildCard; 
    public AudioClip assassinCard; 
    public AudioClip thiefCard; 
    public AudioClip magicianCard; 
    public AudioClip kingCard; 
    public AudioClip bishopCard; 
    public AudioClip merchantCard; 
    public AudioClip architectCard; 
    public AudioClip warlordCard; 
    public AudioClip assassinKill; 
    public AudioClip thiefSteal;
    public AudioClip magicianSwap;
    public AudioClip warlordDestroy;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip nextTurn;






    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CardDealerSound(){
       
        if (cardDealer != null)
        {
            audioSource.PlayOneShot(cardDealer, 1f);
        }
        else
        {
            Debug.LogWarning("cardDealer (AudioClip) не назначен!");
        }
    }

    public void CardPutSound(){
       
        if (cardDealer != null)
        {
            audioSource.PlayOneShot(cardPut);
        }
    }

    public void BuildedSound(){
       
        if (buildCard != null)
        {
            audioSource.PlayOneShot(buildCard, 0.5f);
        }
        else
        {
            Debug.LogWarning("buildCard (AudioClip) не назначен!");
        }
    }
    public void RoleSound(string name){
        if(name == "Assassin" && assassinCard != null)
            audioSource.PlayOneShot(assassinCard, 0.25f);
        else if(name == "Thief" && thiefCard != null)
            audioSource.PlayOneShot(thiefCard);
        else if(name == "Magician" && magicianCard != null)
            audioSource.PlayOneShot(magicianCard, 0.75f);
        else if(name == "King" && kingCard != null)
            audioSource.PlayOneShot(kingCard);
        else if(name == "Bishop" && bishopCard != null)
            audioSource.PlayOneShot(bishopCard, 0.5f);
        else if(name == "Merchant" && merchantCard != null)
            audioSource.PlayOneShot(merchantCard, 0.75f);
        else if(name == "Architect" && architectCard != null)
            audioSource.PlayOneShot(architectCard, 0.75f);
        else if(name == "Warlord" && warlordCard != null)
            audioSource.PlayOneShot(warlordCard);
    }
    public void AssassinKill(){
        if(assassinKill != null)
            audioSource.PlayOneShot(assassinKill);
    }
    public void ThiefSteal(){
        if(thiefSteal != null)
            audioSource.PlayOneShot(thiefSteal);
    }
    public void MagicianSwap(){
        if(magicianSwap != null)
            audioSource.PlayOneShot(magicianSwap);
    }
    public void WarlordDestroy(){
        if(warlordDestroy != null)
            audioSource.PlayOneShot(warlordDestroy);
    }
    public void WinSound(){
        if(winSound != null){
            audioSource.PlayOneShot(winSound);
        }
    }
    public void LoseSound(){
        if(loseSound != null){
            audioSource.PlayOneShot(loseSound);
        }
    }

    public void NextTurnSound(){
        if(nextTurn != null){
            audioSource.PlayOneShot(nextTurn);
        }
    }
}
