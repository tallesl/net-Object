namespace ObjectLibrary.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public class nameTreeTests
    {
        [TestMethod]
        public void WithRoot()
        {
            var tree = new NameTree(
                new[] {
                    "a.i.m.x", "a.i.n.x", "a.i.n.y",
                    "a.j.m.x", "a.j.m.y", "a.j.m.z",
                }
            );

            // level 0
            var a = tree.Root;
            Assert.AreEqual("a", a.Name);
            Assert.AreEqual("a", a.FullName);

            // level 1
            Assert.AreEqual(2, a.Children.Count);

            var ai = a.Children[0];
            Assert.AreEqual("i", ai.Name);
            Assert.AreEqual("a.i", ai.FullName);

            var aj = a.Children[1];
            Assert.AreEqual("j", aj.Name);
            Assert.AreEqual("a.j", aj.FullName);

            // level 2
            Assert.AreEqual(2, ai.Children.Count);
            Assert.AreEqual(1, aj.Children.Count);

            var aim = ai.Children[0];
            Assert.AreEqual("m", aim.Name);
            Assert.AreEqual("a.i.m", aim.FullName);

            var ain = ai.Children[1];
            Assert.AreEqual("n", ain.Name);
            Assert.AreEqual("a.i.n", ain.FullName);

            var ajm = aj.Children[0];
            Assert.AreEqual("m", ajm.Name);
            Assert.AreEqual("a.j.m", ajm.FullName);

            // level 3
            Assert.AreEqual(1, aim.Children.Count);
            Assert.AreEqual(2, ain.Children.Count);
            Assert.AreEqual(3, ajm.Children.Count);

            var aimx = aim.Children[0];
            Assert.AreEqual("x", aimx.Name);
            Assert.AreEqual("a.i.m.x", aimx.FullName);

            var ainx = ain.Children[0];
            Assert.AreEqual("x", ainx.Name);
            Assert.AreEqual("a.i.n.x", ainx.FullName);

            var ainy = ain.Children[1];
            Assert.AreEqual("y", ainy.Name);
            Assert.AreEqual("a.i.n.y", ainy.FullName);

            var ajmx = ajm.Children[0];
            Assert.AreEqual("x", ajmx.Name);
            Assert.AreEqual("a.j.m.x", ajmx.FullName);

            var ajmy = ajm.Children[1];
            Assert.AreEqual("y", ajmy.Name);
            Assert.AreEqual("a.j.m.y", ajmy.FullName);

            var ajmz = ajm.Children[2];
            Assert.AreEqual("z", ajmz.Name);
            Assert.AreEqual("a.j.m.z", ajmz.FullName);
        }

        [TestMethod]
        public void WithoutRoot()
        {
            var tree = new NameTree(
                new[] {
                    // level 0
                    // (empty root)

                    // level 1
                    "i", "j", // missing "k" on purpose

                    // level 2
                    "i.m", "i.n", // missing "k.m" on purpose

                    // level 3
                    "i.m.x",
                    "i.n.x", "i.n.y",
                    "k.m.x", "k.m.y", "k.m.z",
                }
            );

            // level 0
            Assert.AreEqual(string.Empty, tree.Root.Name);
            Assert.AreEqual(string.Empty, tree.Root.FullName);

            // level 1
            Assert.AreEqual(3, tree.Root.Children.Count);

            var i = tree.Root.Children[0];
            Assert.AreEqual("i", i.Name);
            Assert.AreEqual("i", i.FullName);

            var j = tree.Root.Children[1];
            Assert.AreEqual("j", j.Name);
            Assert.AreEqual("j", j.FullName);

            var k = tree.Root.Children[2];
            Assert.AreEqual("k", k.Name);
            Assert.AreEqual("k", k.FullName);

            // level 2
            Assert.AreEqual(2, i.Children.Count);
            Assert.AreEqual(1, k.Children.Count);

            var im = i.Children[0];
            Assert.AreEqual("m", im.Name);
            Assert.AreEqual("i.m", im.FullName);

            var @in = i.Children[1];
            Assert.AreEqual("n", @in.Name);
            Assert.AreEqual("i.n", @in.FullName);

            var km = k.Children[0];
            Assert.AreEqual("m", km.Name);
            Assert.AreEqual("k.m", km.FullName);

            // level 3
            Assert.AreEqual(1, im.Children.Count);
            Assert.AreEqual(2, @in.Children.Count);
            Assert.AreEqual(3, km.Children.Count);

            var imx = im.Children[0];
            Assert.AreEqual("x", imx.Name);
            Assert.AreEqual("i.m.x", imx.FullName);

            var inx = @in.Children[0];
            Assert.AreEqual("x", inx.Name);
            Assert.AreEqual("i.n.x", inx.FullName);

            var iny = @in.Children[1];
            Assert.AreEqual("y", iny.Name);
            Assert.AreEqual("i.n.y", iny.FullName);

            var kmx = km.Children[0];
            Assert.AreEqual("x", kmx.Name);
            Assert.AreEqual("k.m.x", kmx.FullName);

            var kmy = km.Children[1];
            Assert.AreEqual("y", kmy.Name);
            Assert.AreEqual("k.m.y", kmy.FullName);

            var kmz = km.Children[2];
            Assert.AreEqual("z", kmz.Name);
            Assert.AreEqual("k.m.z", kmz.FullName);
        }

        [TestMethod]
        public void Empty()
        {
            var tree = new NameTree(Enumerable.Empty<string>());

            Assert.AreEqual(0, tree.Root.Children.Count);
            Assert.AreEqual(string.Empty, tree.Root.Name);
            Assert.AreEqual(string.Empty, tree.Root.FullName);
        }
    }
}
