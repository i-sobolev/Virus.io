using UnityEngine;
using UnityEngine.UI;

public class UISkinPreview : MonoBehaviour
{
    public Image head;
    public Image[] bodies;

    public Image overlay;

    public Text cost;
    public Image @lock;

    public GameObject maxLevel;

    public UIShopButton currentSelectedSkin;

    public Sprite qIcon;
    public Sprite lockIcon;

    public GameObject purchaseButton;

    public void ShowSkinOnPreviewTable(UIShopButton newSkin)
    {
        currentSelectedSkin = newSkin;

        head.sprite = newSkin.snakeSprites[0];

        @lock.sprite = lockIcon;

        foreach (Image body in bodies)
        {
            body.sprite = newSkin.snakeSprites[1];
            body.color = new Color(1, 1, 1, 1);
        }

        head.color = new Color(1, 1, 1, 1);

        if (!newSkin.isBought)
        {
            overlay.enabled = true;

            purchaseButton.SetActive(true);
            maxLevel.SetActive(false);

            if (newSkin.isSecretSkin)
            {
                //@lock.enabled = true;
                //cost.gameObject.SetActive(false);
                //@lock.sprite = qIcon;

                for (int i = 0; i < bodies.Length; i++)
                {
                    bodies[i].color = SecretSkin.Colors[i];
                }

                head.color = SecretSkin.Colors[0];
            }

            //else if (newSkin.LvlSkin && !newSkin.prevSkinLvl.isBought)
            //{
            //    @lock.enabled = true;
            //    cost.gameObject.SetActive(false);
            //}

            else
            {
                @lock.enabled = false;
            }

            cost.gameObject.SetActive(true);

            cost.text = newSkin.cost.ToString();
        }

        else
        {
            if (newSkin.isSecretSkin)
            {
                for (int i = 0; i < bodies.Length; i++)
                {
                    bodies[i].color = SecretSkin.Colors[i];
                }

                head.color = SecretSkin.Colors[0];
            }

            overlay.enabled = false;
            cost.gameObject.SetActive(false);
            @lock.enabled = false;

            purchaseButton.SetActive(false);

            maxLevel.SetActive(true);
            maxLevel.GetComponentInChildren<Text>().text = "Max LVL: " + UIFinalScorePanel.LevelNames[newSkin.skinMaxLevelInt] + " (" + newSkin.skinMaxLevelInt.ToString() + ")";
        }
    }

    public void SellCurrentSelectedSkin()
    {
        currentSelectedSkin.SetSprites();
    }
}
