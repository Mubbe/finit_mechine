using Godot;
using System;
using System.Net.Http;
using System.Net.NetworkInformation;

public partial class Enemy : Node3D
{
    [Export] public CharacterBody3D player;
    [Export] public CsgMesh3D Head;

    bool now_chasing=false;

    // Called when the node enters the scene tree for the first time.
    enum Npcstate
    {
        idle,
        chasing
    }

    public override void _Ready()
    {
        
        
           var area = GetNode<Area3D>("Area3D");
            area.BodyEntered += BodyEntered;
            area.BodyExited += BodyExited;
        
    }


    private Npcstate currentstate = Npcstate.idle;
    public override void _Process(double delta)
    {
        
        switch (currentstate)
        {
            case Npcstate.idle:
                {
                    Vector3 targetPos = player.GlobalPosition;
                    targetPos.Y = GlobalPosition.Y;



                    float dis = (player.GlobalPosition - GlobalPosition).Length();
                    if (dis <= 10f)
                    {
                        currentstate = Npcstate.chasing;
                        GD.Print("Chasing player...");
                    }



                    break;
                    
                }
            case Npcstate.chasing:
                {

                    // Move towards the player but keep the same Y position:
                    Vector3 targetPos = player.GlobalPosition;
                    targetPos.Y = GlobalPosition.Y;

                    Vector3 dir = (targetPos - GlobalPosition).Normalized();
                    float speed = 2f;
                    GlobalTranslate(dir * speed * (float)delta);


                    float dis = (player.GlobalPosition - GlobalPosition).Length();
                    if (dis >= 10f)
                    {
                        currentstate= Npcstate.idle;
                        GD.Print("idle player...");
                    }
                    
                    
                    break;
                }


        }
    }

    void BodyEntered(Node3D body)
    {
        GD.Print("BodyEntered called with: ", body.Name);
        if (body == player)
        {
            currentstate = Npcstate.chasing;
            
        }
    }

    void BodyExited(Node3D body)
    {
        if (body == player)
        {
            GD.Print("Player exited");
            currentstate = Npcstate.idle;
        }
    }

   


}
