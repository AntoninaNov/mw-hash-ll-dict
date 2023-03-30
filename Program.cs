public class KeyValuePair
{
    public string Key { get; }

    public string Value { get; }

    public KeyValuePair(string key, string value)
    {
        Key = key;
        Value = value;
    }
}

public class LinkedListNode
{
    public KeyValuePair Pair { get; }
        
    public LinkedListNode Next { get; set; }

    public LinkedListNode(KeyValuePair pair, LinkedListNode next = null)
    {
        Pair = pair;
        Next = next;
    }
}

public class LinkedList
{
    private LinkedListNode _first;
    private LinkedListNode _last;
    
    public void Add(KeyValuePair pair)
    {

        var newNode = new LinkedListNode(pair);
        
        if (_first == null)
        {
            _first = newNode;
            _last = newNode;
        }
        else
        {
            _last.Next = newNode;
            _last = newNode;
        }
    }

    public void RemoveByKey(string key)
    {
        if (_first == null) return;

        if (_first.Pair.Key == key)
        {
            _first = _first.Next;
            if (_first == null)
                _last = null;
            return;
        }

        LinkedListNode current = _first;
        while (current.Next != null)
        {
            if (current.Next.Pair.Key == key)
            {
                current.Next = current.Next.Next;
                if (current.Next == null)
                    _last = current;
                return;
            }
            current = current.Next;
        }
    }

    //public KeyValuePair GetFirstNode(string key)
    //{
        
    //}

    //public KeyValuePair GetItemWithKey(string key)
    //{
        // get pair with provided key, return null if not found
    //}
}

// Hashtable - an array that's conceptually vertical, but that horizontally is a linked list
public class StringsDictionary
{
    private const int InitialSize = 10;
    private const double LoadFactorThreshold = 0.75;
    private int _itemsCount = 0;

    private LinkedList[] _buckets = new LinkedList[InitialSize];
        
    public void Add(string key, string value)
    {
        CheckAndResize();
        int hash = CalculateHash(key);
        int index = hash % _buckets.Length;

        if (_buckets[index] == null)
        {
            _buckets[index] = new LinkedList();
        }

        _buckets[index].Add(new KeyValuePair(key, value));
        _itemsCount++;
    }


    //public void Remove(string key)
    //{
    //        
    //}

    public string Get(string key)
    {
        int hash = CalculateHash(key);
        int index = hash % _buckets.Length;

        if (_buckets[index] == null)
            return null;

        KeyValuePair pair = _buckets[index].GetItemWithKey(key);
        return pair?.Value;
    }
    
    private int CalculateHash(string key)
    {
        int currentHashValue = 0;
        int primeMultiplier = 31; // multiplier is 31 for a better distribution

        foreach (char currentCharacter in key)
        {
            int asciiValue = currentCharacter;
            currentHashValue = currentHashValue * primeMultiplier + asciiValue;
        }
        return Math.Abs(currentHashValue);
    }

    // initial size
    // to use enumerator

    //private void CollisionsSolver()
    //{
    //
    //}
    
    private void CheckAndResize()
    {
        double loadFactor = (double)_itemsCount / _buckets.Length;

        if (loadFactor < LoadFactorThreshold)
            return;

        int newSize = _buckets.Length * 2;
        LinkedList[] newBuckets = new LinkedList[newSize];

        for (int i = 0; i < _buckets.Length; i++)
        {
            LinkedList currentList = _buckets[i];
            if (currentList == null)
                continue;

            LinkedListNode currentNode = currentList.GetFirstNode(); // node[0]
            while (currentNode != null)
            {
                int newHash = CalculateHash(currentNode.Pair.Key);
                int newIndex = newHash % newSize;

                if (newBuckets[newIndex] == null)
                    newBuckets[newIndex] = new LinkedList();

                newBuckets[newIndex].Add(currentNode.Pair);
                currentNode = currentNode.Next;
            }
        }
        _buckets = newBuckets;
    }

    public void LoadFromFile(string pathToFile)
    {
        foreach (var line in File.ReadAllLines(pathToFile))
        {
            string[] elements = line.Split("; ");
            string key = elements[0];
            string value = String.Join("; ", elements[1..]);
            Add(key, value);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        StringsDictionary dictionary = new StringsDictionary();
        dictionary.LoadFromFile("dictionary.txt");
    }
}