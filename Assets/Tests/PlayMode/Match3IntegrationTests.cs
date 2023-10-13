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
        SceneManager.LoadScene("Match3IntegrationTestScene", LoadSceneMode.Additive);

        yield return new WaitForSeconds(1f);

        var lifetimeScope = Object.FindObjectOfType<IntegrationTestsLifetimeScope>();
        Assert.IsNotNull(lifetimeScope, "Could not find LifetimeScope in the scene.");
        _resolver = lifetimeScope.Container;
    }

    [UnityTearDown]
    public IEnumerator UnityTearDown()
    {
        yield return SceneManager.UnloadSceneAsync("Match3IntegrationTestScene");
        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    [Category("LongRunning")]
    [Timeout(1000 * 60 * 60 * 24)]
    public IEnumerator Game_With1000000Swipes_WorksWithoutAnyExceptions()
    {
        var gameController = _resolver.Resolve<TestGameController>();
        Assert.IsNotNull(gameController, "Could not resolve YourType from VContainer.");

        gameController.Start();

        for (int i = 0; i < 1000000; i++)
        {
            gameController.DoSwipe();
            yield return new WaitForSeconds(0.5f);
            Debug.Log($"Move: {i}");
        }

        Assert.Pass();
    }
}