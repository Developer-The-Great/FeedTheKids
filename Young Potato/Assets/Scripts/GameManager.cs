using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{

    private static GameManager m_GameManager;

    private InputController m_InputController;

    private GameObject gameObject;

    public static GameManager Manager
    {
        get
        {
            if (m_GameManager == null)
            {
                m_GameManager = new GameManager();
                m_GameManager.gameObject = new GameObject();
                m_GameManager.gameObject.AddComponent<InputController>();
            }

            return m_GameManager;
        }

    }

    public InputController Input
    {
        get
        {
            if(m_InputController == null)
            {
                m_InputController = m_InputController = gameObject.GetComponent<InputController>();
            }
            return m_InputController;
        }
    }

}

   

    

