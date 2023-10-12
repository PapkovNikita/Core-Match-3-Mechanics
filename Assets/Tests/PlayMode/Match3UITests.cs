using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using VContainer;
using Object = UnityEngine.Object;

public class Match3UITests
{
    private IObjectResolver _resolver;


    [UnitySetUp]
    public IEnumerator UnitySetUp()
    {
        SceneManager.LoadScene("Match3UITestScene");

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