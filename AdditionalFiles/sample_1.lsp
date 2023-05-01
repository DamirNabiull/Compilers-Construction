(func fibonacci (n)
    (cond (greater n 1)
        (return (plus (fibonacci (minus n 1))
            (fibonacci (minus n 2))))
        (return n)))

(fibonacci +4)