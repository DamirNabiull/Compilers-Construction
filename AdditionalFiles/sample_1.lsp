(func fibonacci (n)
    (cond (greater n 1)
        (plus (fibonacci (minus n 1))
            (fibonacci (minus n 2)))
        n))

(print (fibonacci +4))