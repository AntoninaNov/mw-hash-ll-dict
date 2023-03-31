using System;
using System.IO;
using System.Linq;
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
    
    public int Count { get; private set; }
    
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
        Count++;
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
        Count--;
    }

    public KeyValuePair GetItemWithKey(string key)
    {
        LinkedListNode current = _first;
        while (current != null)
        {
            if (current.Pair.Key == key)
            {
                return current.Pair;
            }
            current = current.Next;
        }
        return null;
    }

    public LinkedListNode GetFirstNode()
    {
        return _first;
    }
}

// Hashtable - an array that's conceptually vertical, but that horizontally is a linked list
public class StringsDictionary
{
    private const int InitialSize = 10;
    private const double LoadFactorThreshold = 0.75;
    private int _itemsCount = 0;
    private int _nonEmptyBucketsCount = 0;

    private LinkedList[] _buckets = new LinkedList[InitialSize];
        
    public void Add(string key, string value)
    {
        CheckAndResize();
        int hash = CalculateHash(key);
        int index = hash % _buckets.Length;

        if (_buckets[index] == null)
        {
            _buckets[index] = new LinkedList();
            _nonEmptyBucketsCount++;
        }

        _buckets[index].Add(new KeyValuePair(key, value));
        _itemsCount++;
    }


    public void Remove(string key)
    {
        int hash = CalculateHash(key);
        int index = hash % _buckets.Length;
        // if valid key and if linked list exist, then remove element
        if (key != null && _buckets[index] != null)
        {
            int initialCount = _buckets[index].Count;
            _buckets[index].RemoveByKey(key);
            if (_buckets[index].Count < initialCount)
            {
                if (_buckets[index].Count == 0)
                {
                    _nonEmptyBucketsCount--;
                }
            }
        }
    }

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
        if (IsResizeNeeded())
        {
            ResizeBuckets();
        }
    }

    private bool IsResizeNeeded()
    {
        double loadFactor = (double)_nonEmptyBucketsCount / _buckets.Length;
        return loadFactor >= LoadFactorThreshold;
    }

    private void ResizeBuckets()
    {
        int newSize = _buckets.Length * 2;
        LinkedList[] newBuckets = new LinkedList[newSize];

        foreach (LinkedList currentList in _buckets)
        {
            if (currentList == null)
                continue;

            LinkedListNode currentNode = currentList.GetFirstNode();
            while (currentNode != null)
            {
                KeyValuePair pair = currentNode.Pair;
                AddPairToNewBuckets(pair, newBuckets);
                currentNode = currentNode.Next;
            }
        }
        _buckets = newBuckets;
    }

    private void AddPairToNewBuckets(KeyValuePair pair, LinkedList[] newBuckets)
    {
        int newHash = CalculateHash(pair.Key);
        int newIndex = newHash % newBuckets.Length;

        if (newBuckets[newIndex] == null)
        {
            newBuckets[newIndex] = new LinkedList();
            _nonEmptyBucketsCount++;
        }

        newBuckets[newIndex].Add(pair);
    }
    
    public void DisplayBucketInfo()
    {
        double loadFactor = (double)_nonEmptyBucketsCount / _buckets.Length;
        Console.WriteLine($"Total buckets: {_buckets.Length}");
        Console.WriteLine($"Non-empty buckets: {_nonEmptyBucketsCount}");
        Console.WriteLine($"Load factor: {loadFactor}");
    }
    
    public int NewWordsToReachTargetLoadFactor()
    {
        double currentLoadFactor = (double)_nonEmptyBucketsCount / _buckets.Length;
        double targetLoadFactor = 0.75;
        if (currentLoadFactor >= targetLoadFactor)
        {
            return 0;
        }

        int requiredNonEmptyBuckets = (int)Math.Ceiling(_buckets.Length * targetLoadFactor);
        int newWordsNeeded = requiredNonEmptyBuckets - _nonEmptyBucketsCount;
        return newWordsNeeded;
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
        static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        StringsDictionary dictionary = new StringsDictionary();
        dictionary.LoadFromFile("/Users/antoninanovak/RiderProjects/custom-dictionary-mw/dictionary.txt");
        
        Console.WriteLine("Initial bucket information:");
        dictionary.DisplayBucketInfo();

        while (true)
        {
            Console.Write("Enter a command (search, add, delete, random, exit): ");
            string command = Console.ReadLine().ToLower();

            if (command == "exit")
            {
                break;
            }

            if (command == "search")
            {
                Console.Write("Enter a word: ");
                string word = Console.ReadLine();
                string definition = dictionary.Get(word.ToUpper());
                if (definition != null)
                {
                    Console.WriteLine($"Definition for '{word}': {definition}");
                }
                else
                {
                    Console.WriteLine($"The word '{word}' was not found in the dictionary.");
                }
            }
            else if (command == "add")
            {
                Console.Write("Enter a word: ");
                string word = Console.ReadLine();
                Console.Write("Enter the definition: ");
                string definition = Console.ReadLine();
                dictionary.Add(word.ToUpper(), definition);
                Console.WriteLine($"The word '{word}' with definition '{definition}' has been added to the dictionary.");
                Console.WriteLine("Bucket information after adding a word:");
                dictionary.DisplayBucketInfo();
            }
            else if (command == "delete")
            {
                Console.Write("Enter a word: ");
                string word = Console.ReadLine();
                dictionary.Remove(word.ToUpper());
                Console.WriteLine($"The word '{word}' has been removed from the dictionary.");
                Console.WriteLine("Bucket information after removing a word:");
                dictionary.DisplayBucketInfo();
            }
            else if (command == "random")
            {
                Console.WriteLine($"New words needed to reach 75% load factor: {dictionary.NewWordsToReachTargetLoadFactor()}");
                Console.Write("Enter a number of random words you want to insert: ");
                int n = int.Parse(Console.ReadLine());
                for (int i = 0; i < n; i++)
                {
                    string randomWord = RandomString(5); // Generate a random word with a length of 5
                    string randomDefinition = RandomString(10); // Generate a random definition with a length of 10
                    dictionary.Add(randomWord.ToUpper(), randomDefinition);
                }
                Console.WriteLine($"Bucket information after inserting {n} random word(s): ");
                dictionary.DisplayBucketInfo();
            }
            else
            {
                Console.WriteLine("Invalid command. Please enter 'search', 'add', 'delete', 'random', or 'exit'.");
            }
        }
    }
}
// KSE
// 1. the best university; 2. alma mater 
