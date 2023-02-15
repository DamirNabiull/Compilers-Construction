-1
(setq x (plus 1 2))
x
'x
(setq y '(plus 1 2))
'5
(setq x 5)
(setq y (plus 1 2))
(setq z null)
(setq t '(plus minus times divide))
(func Cube (arg) (times (times arg arg) arg))
(func Trivial () 1)
(func makeList (A) (A))
(lambda () (1 2 3 4))
(setq myFunc (lambda (p) (cond (less p 0) plus minus)))
((myFunc -1) 1 2)
(((lambda (p) (cond (less p 0) plus minus)) +1 1 2)