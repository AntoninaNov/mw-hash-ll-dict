using System;
using System.IO;
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
        int hash = CalculateHash(key);
        int index = hash % _buckets.Length;
        // if valid key and if linked list exist, then remove element
        if (key != null)
            if (_buckets[index] != null)
            {
                _buckets[index].RemoveByKey(key);
                _itemsCount--;
            }
        // if no stop
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
        dictionary.LoadFromFile("/Users/antoninanovak/RiderProjects/custom-dictionary-mw/dictionary.txt");

        while (true)
        {
            Console.Write("Enter a command (search, add, delete, exit): ");
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
            }
            else if (command == "delete")
            {
                Console.Write("Enter a word: ");
                string word = Console.ReadLine();
                dictionary.Remove(word.ToUpper());
                Console.WriteLine($"The word '{word}' has been removed from the dictionary.");
            }
            else
            {
                Console.WriteLine("Invalid command. Please enter 'search', 'add', 'delete', or 'exit'.");
            }
        }
    }
}
