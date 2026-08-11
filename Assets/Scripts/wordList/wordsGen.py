import json

def validWord(word):
    lastLetter = ""
    for i in word:
        if lastLetter == i:
            return False
        lastLetter = i
    return True

with open("dictionaryFull.txt") as f:
    wordsSrc = f.read().split("\n") 

output = {}
for word in wordsSrc:
    if len(word) != 0 and validWord(word):
        if len(word) in output:
            output[len(word)].append(word)
        else:
            output[len(word)] = [word]

with open("wordList.json", "w") as f:
    f.write(json.dumps(output))

