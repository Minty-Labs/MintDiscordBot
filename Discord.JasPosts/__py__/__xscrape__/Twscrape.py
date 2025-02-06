from twscrape import API, gather
from twscrape.logger import set_log_level
import asyncio
import datetime
import json
import os
from pathlib import Path
from dotenv import load_dotenv

load_dotenv()

TWITTER_ID = int(os.getenv("TWITTER_ID"))

def Enquiry(lis1): 
    if len(lis1) == 0: 
        return 0
    else: 
        return 1


async def main():
    try:
        monitorID = TWITTER_ID # Target Twitter Account
        api = API()  # or API("path-to.db") - default is `accounts.db`
		
        await api.pool.login_all()
		
        doc = await gather(api.user_tweets(monitorID, limit=1))
        Tweet1 = doc[0].json()  # -> json string
        Tweet2 = doc[1].json()
        Tweet3 = doc[2].json()
        Tweet4 = doc[3].json()
        Tweet5 = doc[4].json()
        Tweet6 = doc[5].json()
        Tweet7 = doc[6].json()
        Tweet8 = doc[7].json()
        Tweet9 = doc[8].json()
        Tweet10 = doc[9].json()

        array_tweets = [Tweet1,Tweet2,Tweet3, Tweet4, Tweet5,Tweet6, Tweet7, Tweet8, Tweet9, Tweet10]
        new_array_tweets = []

        # Get Twitter IDs that are seen already
        lines2 = []
        ID_to_be_saved = 0

        TweetIDFile = Path("TweetID.txt")
        if TweetIDFile.is_file():
            with open("TweetID.txt", "r") as file2:
                lines2 = [line.rstrip() for line in file2]
            file2.close()

        print("=========================================================================")
        print(str(datetime.datetime.now()))
        print("=========================================================================")
        for x in array_tweets:
            JSONTweet1 = json.loads(x)
            print("Tweet ID: " + str(JSONTweet1["id"]))
            
            if str(JSONTweet1["id"]) in lines2:
                print("ID In List")
            else:
                print("Not in list")
                if int(JSONTweet1["user"]["id"]) == monitorID:

                    QuoteTweet = JSONTweet1["quotedTweet"]
                    RetweetTweet = JSONTweet1["retweetedTweet"]

                    if RetweetTweet is not None and QuoteTweet is not None:
                        print("Confusing Tweet")
                    elif RetweetTweet is not None:
                        print("Is Retweet")
                    elif QuoteTweet is not None:
                        print("Is Quote Tweet")
                    else:
                        base_url = str(JSONTweet1["url"])
                        modified_url = "https://fixupx" + base_url[9:]

                        x = {
                            "id": str(JSONTweet1["id"]),
                            "URL": modified_url
                        }
                        final_json = json.dumps(x)

                        with open("data.json", "w+") as filewrite:
                            filewrite.write(f"{final_json}\n")


                        print(final_json)

                        ID_to_be_saved = str(JSONTweet1["id"])
                    new_array_tweets.append(JSONTweet1["id"])

                else:
                    print("Tweet by: " + str(JSONTweet1["user"]["displayname"]))
                    print("Not Tweet of User")

            new_array_tweets.append(JSONTweet1["id"])
            print("=========================================================================")

        if ID_to_be_saved == 0:
            x = {
                "id": str(0),
                "URL": "None"
            }
            final_json = json.dumps(x)
            with open("data.json", "w+") as filewrite:
                filewrite.write(f"{final_json}\n")
        else:
            lines2.append(ID_to_be_saved)

        with open("TweetID.txt", "w+") as filewrite2:
            for line in lines2:
                filewrite2.write(f"{line}\n")
        filewrite2.close()

        print("Success check tweets at " + str(datetime.datetime.now()))
    except Exception as err : 
        print(str(err) + " - " + str(datetime.datetime.now()))

if __name__ == "__main__":
    asyncio.run(main())