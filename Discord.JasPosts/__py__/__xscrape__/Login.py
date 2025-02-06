import argparse
import asyncio
from twscrape import API

async def main(username, password, email, phone):
    api = API()  # or API("path-to.db") - default is `accounts.db`
    # ADD ACCOUNTS
    await api.pool.add_account(username, password, email, phone)
    await api.pool.login_all()

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Login script with parameters.")
    parser.add_argument("username", help="Twitter username")
    parser.add_argument("password", help="Twitter password")
    parser.add_argument("email", help="Email associated with the Twitter account")
    parser.add_argument("phone", help="Phone number associated with the Twitter account")
    
    args = parser.parse_args()
    
    asyncio.run(main(args.username, args.password, args.email, args.phone))
