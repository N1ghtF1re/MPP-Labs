using NUnit.Framework;

namespace MPP9.tests
{
    [TestFixture]
    public class DynamicListTest
    {
        private DynamicList<int> _list;

        [SetUp]
        public void SetUp()
        {
            _list = new DynamicList<int>();
        }
        
        [Test]
        public void AddTest()
        {
            _list.Add(2);
            
            Assert.That(_list.Count, Is.EqualTo(1));
            Assert.That(_list.Items[0], Is.EqualTo(2));
        }
        
        
        [Test]
        public void RemoveAtTest()
        {
            for (var i = 0; i < 4; i++)
            {
                _list.Add(i);
            }
            
            _list.RemoveAt(2);
            
            Assert.That(_list.Count, Is.EqualTo(3));
            Assert.That(_list.Items, Is.EqualTo(new int[] {0,1,3}));
        }
        
        [Test]
        public void RemoveTest()
        {
            for (var i = 4; i > 0; i--)
            {
                _list.Add(i);
            }
            
            bool removed = _list.Remove(2);
            
            Assert.That(removed, Is.True);
            
            Assert.That(_list.Count, Is.EqualTo(3));
            Assert.That(_list.Items, Is.EqualTo(new int[] {4,3,1}));
            
            removed = _list.Remove(2);
            Assert.That(removed, Is.False);
        }
        
        [Test]
        public void ClearTest()
        {
            for (var i = 4; i > 0; i--)
            {
                _list.Add(i);
            }
            
            _list.Clear();
            Assert.That(_list.Count, Is.EqualTo(0));
            Assert.That(_list.Items, Is.EqualTo(new int[]{}));
            
            _list.Add(1);
            Assert.That(_list.Count, Is.EqualTo(1));
            Assert.That(_list.Items[0], Is.EqualTo(1));
        }

        [Test]
        public void ExtendingTest()
        {
            _list = new DynamicList<int>(4);
            for (int i = 0; i < 400; i++)
            {
                _list.Add(i);
            }
            
            Assert.That(_list.Count, Is.EqualTo(400));
            Assert.That(_list.Items.Length, Is.EqualTo(400));
        }

        [Test]
        public void EnumeratorTest()
        {
            for (int i = 0; i < 11; i++)
            {
                _list.Add(i);
            }

            int index = 0;
            using (var enumerator  = _list.GetEnumerator())
            {
                while (enumerator.MoveNext()) { 
                    int val = enumerator.Current; 
                    Assert.That(val, Is.EqualTo(index++));
                } 
            }
            Assert.That(index, Is.EqualTo(11));
            
        }
    }
}