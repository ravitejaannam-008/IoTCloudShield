# IoTCloudShield
A simulator to secure IoT devices in an AWS cloud environment.

## Features
- Simulates IoT traffic with AWS IoT Core
- Enforces TLS encryption and device auth
- Detects and mitigates MITM attacks

## Tech Stack
- AWS IoT Core, Python 3.9 (paho-mqtt)
- OpenSSL (certificate generation)

## Setup
1. Clone the repo: git clone https://github.com/ravitejaannam-008/IoTCloudShield.git cd IoTCloudShield
2. Install dependencies: pip install -r requirements.txt
3. Set up AWS IoT Core and download certs.

## Usage
Start the simulator: python simulate.py --cert certs/device.pem --key certs/private.key

## Example
$ python simulate.py --cert certs/device.pem Connected 5 devices—MITM attempt blocked Logs saved to logs/iot.log
## Why This Matters
Fuses my cloud and networking skills into IoT security—a frontier I’m excited to secure.
