using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using TMPro;
using UnityEngine.SceneManagement;


namespace Tests
{
    public class TestSuite
    {
      private GameObject testObject;
      private pauseScript pauseMono;
      public GameObject pauseMenuUI;

        [SetUp]
        public void Setup()
        {
          //SceneManager.LoadScene("Single Player");
          //yield return null;

          testObject = GameObject.Instantiate(new GameObject());
          pauseMono = testObject.AddComponent<pauseScript>();
          //gameScene.Find
        }

        [UnityTest]
        public IEnumerator TestPauseButton()
        {
          Assert.IsFalse(pauseMono.GetPauseStatus());
          //Input.GetKeyDown(KeyCode.P);
          pauseMono.SetPauseStatus(true);
          yield return new WaitForSeconds(0.1f);
          Assert.IsTrue(pauseMono.GetPauseStatus());

          pauseMono.SetPauseStatus(false);
          yield return new WaitForSeconds(0.1f);
          Assert.IsFalse(pauseMono.GetPauseStatus());

          yield return null;
        }

        [UnityTest]
        public IEnumerator PauseToMainMenu()
        {
          //Assert.IsFalse(pauseMono.GetPauseStatus());

          //pauseMono.SetPauseStatus(true);
          //pauseMono.mainMenu();
          //Debug.Log("Scene Name" + SceneManager.GetActiveScene().name);
          //Assert.IsTrue(SceneManager.GetActiveScene().name,"Main Menu");
          yield return null;
        }

        [TearDown]
        public void TearDown()
        {
          GameObject.Destroy(testObject);
        }
    }
}
