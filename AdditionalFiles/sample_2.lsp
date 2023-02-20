(func gcd2(a b)
    (cond(equal b 0) 
        a
        (gcd2 b (mod a b))))

(print (gcd2 150 50))