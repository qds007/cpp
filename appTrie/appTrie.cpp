#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>

// inline Macros
#define ARRAY_SIZE(a) sizeof(a)/sizeof(a[0])
#define ALPHABET_SIZE (26)
#define CHAR_TO_INDEX(c) ((int)c - (int)'a') // ASCII for 'a' is 97
inline int charToPos(char alphabet) { return (int)alphabet-97; }
typedef struct TrieNode Dictionary;

struct TrieNode
{
	struct TrieNode *children[ALPHABET_SIZE];
	bool isLeaf;
};

struct TrieNode *getNode(void)
{
	struct TrieNode *pNode = NULL;  // sizeof(pNode->children)=208=26*8, pointer is b bytes in 64 bit systen
	                                // NULL is a macro, right click peek def shows that.
	
	// sizeof(struct TrieNode)=216 =208 +8 for bool 
	pNode = (struct TrieNode *)malloc(sizeof(struct TrieNode)); // pNode point to 26 address + 1 bool
	// for-loop point children to NULL

	return pNode;
}

void insert(struct TrieNode *root, const char *key)  //key looks like word to me, eg, "the"
{
	int level;
	size_t length = strlen(key);
	int index;

	struct TrieNode *pCrawl = root;

	for (level = 0; level < length; level++)
	{
		index = charToPos(key[level]); // as level increase, key move with it, e.g. "the" level=0 is 't', level 2 is 'e'

		if (!pCrawl->children[index])   //index is horizontal move 0='a' 1='b' .... 25='z'
			pCrawl->children[index] = getNode();  // at the correctly indexed into children attach new node

		pCrawl = pCrawl->children[index];
	}

	pCrawl->isLeaf = true;
}
bool search(struct TrieNode *root, const char *key)
{
	// search means walk through levels and horizontally through children array
	// if not found, then somewhere much walk onto a child never attached a new node
	return false;
}

int main()
{
	Dictionary *root = getNode();

	printf("%d",charToPos('b'));
	getchar();
    return 0;
}

