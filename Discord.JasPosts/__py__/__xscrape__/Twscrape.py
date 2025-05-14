from twscrape import API, gather
# from twscrape.logger import set_log_level
import asyncio
import datetime
import json
import os
from pathlib import Path
from dotenv import load_dotenv

load_dotenv()

twitter_id = int(os.getenv("TWITTER_ID"))

def enquiry(lis1): 
    if len(lis1) == 0: 
        return 0
    else: 
        return 1

async def main():
    try:
        monitor_id = twitter_id # Target Twitter Account
        api = API()  # or API("path-to.db") - default is `accounts.db`
        
        await api.pool.login_all()
        
        document = await gather(api.user_tweets(monitor_id, limit=1))
        tweet1 = document[0].json()  # -> json string
        tweet2 = document[1].json()
        tweet3 = document[2].json()
        tweet4 = document[3].json()
        tweet5 = document[4].json()
        tweet6 = document[5].json()
        tweet7 = document[6].json()
        tweet8 = document[7].json()
        tweet9 = document[8].json()
        tweet10 = document[9].json()

        array_tweets = [ tweet1, tweet2, tweet3, tweet4, tweet5, tweet6, tweet7, tweet8, tweet9, tweet10 ]
        new_array_tweets = []

        # Get Twitter IDs that are seen already
        lines = []
        id_to_be_saved = 0

        tweet_id_file = Path("TweetID.txt")
        if tweet_id_file.is_file():
            with open("TweetID.txt", "r") as file2:
                lines = [line.rstrip() for line in file2]
            file2.close()

        print("=========================================================================".encode("utf-8"))
        print(str(datetime.datetime.now()).encode("utf-8"))
        print("=========================================================================".encode("utf-8"))
        for x in array_tweets:
            json_tweet1 = json.loads(x)
            print(("Tweet ID: " + str(json_tweet1["id"])).encode("utf-8"))
            
            if str(json_tweet1["id"]) in lines:
                print("ID In List".encode("utf-8"))
            else:
                print("Not in list".encode("utf-8"))
                if int(json_tweet1["user"]["id"]) == monitor_id:

                    quote_tweet = json_tweet1["quotedTweet"]
                    retweet_tweet = json_tweet1["retweetedTweet"]

                    if retweet_tweet is not None and quote_tweet is not None:
                        print("Confusing Tweet".encode("utf-8"))
                    elif retweet_tweet is not None:
                        print("Is Retweet".encode("utf-8"))
                    elif quote_tweet is not None:
                        print("Is Quote Tweet".encode("utf-8"))
                    else:
                        base_url = str(json_tweet1["url"])
                        modified_url = "https://fixupx" + base_url[9:]

                        x = {
                            "id": str(json_tweet1["id"]),
                            "URL": modified_url
                        }
                        final_json = json.dumps(x)

                        with open("data.json", "w+") as data_file:
                            data_file.write(f"{final_json}\n")

                        print(final_json.encode("utf-8"))

                        id_to_be_saved = str(json_tweet1["id"])
                    new_array_tweets.append(json_tweet1["id"])
                else:
                    print(("Tweet by: " + str(json_tweet1["user"]["displayname"])).encode("utf-8"))
                    print("Not Tweet of User".encode("utf-8"))

            new_array_tweets.append(json_tweet1["id"])
            print("=========================================================================".encode("utf-8"))

        if id_to_be_saved == 0:
            x = {
                "id": str(0),
                "URL": "None"
            }
            final_json = json.dumps(x)
            with open("data.json", "w+") as data_file:
                data_file.write(f"{final_json}\n")
        else:
            lines.append(id_to_be_saved)

        with open("TweetID.txt", "w+") as tweet_id_file:
            for line in lines:
                tweet_id_file.write(f"{line}\n")
        tweet_id_file.close()

        print(("Success check tweets at " + str(datetime.datetime.now())).encode("utf-8"))
    except Exception as err : 
        print((str(err) + " - " + str(datetime.datetime.now())).encode("utf-8"))

if __name__ == "__main__":
    asyncio.run(main())
