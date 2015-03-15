using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class PlayerInventory
    {
        IList<Armor> myArmors;
        IList<Weapon> myWeapons;
        IList<OtherItem> myOtherItems;
        IList<Skill> mySkills;
        public int myMoney;

        public PlayerInventory()
        {
            myArmors = new List<Armor>();
            myWeapons = new List<Weapon>();
            myOtherItems = new List<OtherItem>();
            mySkills = new List<Skill>();
        }
        public void AddArmor(Armor newArmor)
        {
            myArmors.Add(newArmor);
        }
        public void RemoveArmor(Armor newArmor)
        {
            myArmors.Remove(newArmor);
        }
        public void AddWeapon(Weapon newWeapon)
        {
            myWeapons.Add(newWeapon);
        }
        public void RemoveWeapon(Weapon newWeapon)
        {
            myWeapons.Remove(newWeapon);
        }
        public void AddOtherItem(OtherItem newOtherItem)
        {
            myOtherItems.Add(newOtherItem);
        }
        public void RemoveOtherItem(OtherItem newOtherItem)
        {
            myOtherItems.Remove(newOtherItem);
        }
        public void AddSkill(Skill newSkill)
        {
            mySkills.Add(newSkill);
        }
        public void RemoveSkill(Skill newSkill)
        {
            mySkills.Remove(newSkill);
        }
        public void AddMoney(int money)
        {
            myMoney += money;
        }
        public bool ReduceMoney(int money)
        {
            if (myMoney >= money)
            {
                myMoney -= money;
                return true;
            }
            else
                return false;
        }
        
    }
}
