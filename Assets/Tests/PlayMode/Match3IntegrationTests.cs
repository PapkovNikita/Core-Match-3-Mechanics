using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using VContainer;

public class Match3IntegrationTests
{
    private IObjectResolver _resolver;


    [UnitySetUp]
    public IEnumerator UnitySetUp()
    {
        SceneManager.LoadScene("Match3IntegrationTestScene");

        yield return new WaitForSeconds(1f);

        var lifetimeScope = Object.FindObjectOfType<UITestsLifetimeScope>();
        Assert.IsNotNull(lifetimeScope, "Could not find LifetimeScope in the scene.");
        _resolver = lifetimeScope.Container;
    }

    [UnityTest]
    [Category("LongRunning")]
    public IEnumerator Game_With1000000Swipes_WorksWithoutAnyExceptions()
    {
        var gameController = _resolver.Resolve<TestGameController>();
        Assert.IsNotNull(gameController, "Could not resolve YourType from VContainer.");

        gameController.Start();

        for (int i = 0; i < 100; i++)
        {
            gameController.DoSwipe();
            yield return new WaitForSeconds(0.5f);
        }

        Assert.Pass();
    }
}