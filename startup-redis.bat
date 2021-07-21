@echo off
title redis-server
set ENV_HOME="D:\Redis"
D:
color 03
cd %ENV_HOME%
redis-server.exe redis.windows.conf
exit