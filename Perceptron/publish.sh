#!/usr/bin/bash

dotnet publish -r linux-arm -c Release --self-contained false
tarnet bin/Release/netcoreapp2.1/linux-arm
scp Perceptron.net root@192.168.8.115:/home/root/netcore