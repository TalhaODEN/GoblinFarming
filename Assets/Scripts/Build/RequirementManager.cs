using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirementManager : MonoBehaviour
{
    public enum RequirementType
    {
        Item,
        Gold 
    }
    [System.Serializable] 
    public class Requirement
    {
        public Sprite icon; 
        public int count;
        public RequirementType requirementType;
        public Requirement(Sprite icon, int count,RequirementType type)
        {
            this.icon = icon;
            this.count = count;
            this.requirementType = type;
        }
    }
    [SerializeField]
    public List<Requirement> requirements=new List<Requirement>();
    public static RequirementManager requirementManager;
    public Player player;
    public GameObject prefab;
    private void Awake()
    {
        if(requirementManager == null)
        {
            requirementManager = this;
        }
    }
    public bool CheckRequirements()
    {
        var slots = player.inventory.slots;
        var goldAmount = GoldPanel.goldAmount;
        foreach (var requirement in requirements)
        {
            if (requirement.requirementType == RequirementType.Gold)
            {
                if (goldAmount < requirement.count)
                {
                    return false;
                }
            }
            else
            {
                int currentCount = 0;
                foreach (var slot in slots)
                {
                    if (slot.icon == requirement.icon)
                    {
                        currentCount += slot.itemCount;
                    }
                }

                if (currentCount < requirement.count)
                {
                    return false;
                }
            }
        }
        return true;
    }






}
