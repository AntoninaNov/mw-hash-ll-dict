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
        // add provided pair to the end of the linked list
        
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
        // remove pair with provided key
    }

    public KeyValuePair GetItemWithKey(string key)
    {
        // get pair with provided key, return null if not found
    }
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


    public void Remove(string key)
    {
            
    }

    public string Get(string key)
    {
            
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

    private void CheckAndResize()
    {
        
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