using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviorExecutorBlobber : CharacterBehaviorExecutorPlayer
{
    protected List<SavedObject> team = new List<SavedObject>(8);

    private int current_character;

    public override void DoOnStart()
    {
        base.DoOnStart();
    }

    public void AddTeamMember(SavedObject new_member)
    {
        team.Add(new_member);
    }
    public void AddTeamMember(SavedObject new_member, int index)
    {
        team.Insert(index, new_member);
    }

    private void TeamScrollLeft()
    {
        if(current_character == 0)
        {
            current_character = 7;
        }
        else
        {
            current_character -= 1;
        }
    }

    private void TeamScrollRight()
    {
        if(current_character == 7)
        {
            current_character = 0;
        }
        else
        {
            current_character += 1;
        }
    }

    public override void CycaBlyat()
    {
        // Diddly-squat
    }

    public override void ControlLean()
    {
        // Also nothing
    }
}
