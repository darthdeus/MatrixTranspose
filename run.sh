#!/bin/bash

set -x

mkdir -p graph

DIR=./bin/Release

echo -e "
$DIR/MatrixTranspose.exe NAIVE SIM | ./cachesim 64 64 > graph/naive-64-64.txt
$DIR/MatrixTranspose.exe SMART SIM | ./cachesim 64 64 > graph/smart-64-64.txt

$DIR/MatrixTranspose.exe NAIVE SIM | ./cachesim 64 1024 > graph/naive-64-1024.txt
$DIR/MatrixTranspose.exe SMART SIM | ./cachesim 64 1024 > graph/smart-64-1024.txt

$DIR/MatrixTranspose.exe NAIVE SIM | ./cachesim 64 4096 > graph/naive-64-4096.txt
$DIR/MatrixTranspose.exe SMART SIM | ./cachesim 64 4096 > graph/smart-64-4096.txt

$DIR/MatrixTranspose.exe NAIVE SIM | ./cachesim 512 512 > graph/naive-512-512.txt
$DIR/MatrixTranspose.exe SMART SIM | ./cachesim 512 512 > graph/smart-512-512.txt

$DIR/MatrixTranspose.exe NAIVE SIM | ./cachesim 4096 64 > graph/naive-4096-64.txt
$DIR/MatrixTranspose.exe SMART SIM | ./cachesim 4096 64 > graph/smart-4096-64.txt" | parallel -j6