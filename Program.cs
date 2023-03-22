﻿public class KeyValuePair
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

    public void Add(KeyValuePair pair)
    {
        // add provided pair to the end of the linked list
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

public class StringsDictionary
{
    private const int InitialSize = 10;

    private LinkedList[] _buckets = new LinkedList[InitialSize];
        
    public void Add(string key, string value)
    {
            
    }

    public void Remove(string key)
    {
            
    }

    public string Get(string key)
    {
            
    }


    private int CalculateHash(string key)
    {
        // function to convert string value to number 
    }
}
