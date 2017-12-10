#!/bin/bash

for f in graph/*.txt; do
    paste <(grep Elements: $f | cut -d' ' -f2) <(grep Misses: $f | cut -d' ' -f2) <(grep Accesses: $f | cut -d' ' -f2) | awk '{ print $1, $2 / ($3 / 2) }'  > ${f%.txt}.graph
done