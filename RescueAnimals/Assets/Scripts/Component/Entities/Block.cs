 u s i n g   S y s t e m ;  
 u s i n g   E n t i t i e s ;  
 u s i n g   U n i t y E n g i n e ;  
 u s i n g   U t i l ;  
  
 p u b l i c   c l a s s   B l o c k   :   M o n o B e h a v i o u r ,   I P o o l a b l e < B l o c k >  
 {  
         [ S e r i a l i z e F i e l d ]   p r i v a t e   d o u b l e   M a x H p   =   1 0 ;  
         [ S e r i a l i z e F i e l d ]   p r i v a t e   d o u b l e   H p   =   1 0 ;  
         [ S e r i a l i z e F i e l d ]   p r i v a t e   A n i m a t o r   _ a n i m a t o r ;  
          
         p u b l i c   A c t i o n   O n B l o c k D e s t r o y e d ;  
         p r i v a t e   A c t i o n < B l o c k >   _ r e t u r n T o P o o l ;  
  
         p r i v a t e   v o i d   A w a k e ( )  
         {  
                 _ a n i m a t o r   =   G e t C o m p o n e n t < A n i m a t o r > ( ) ;  
         }  
  
         p r i v a t e   v o i d   O n C o l l i s i o n E n t e r 2 D ( C o l l i s i o n 2 D   c o l l i s i o n )  
         {  
                 v a r   a t t a c k a b l e   =   c o l l i s i o n . g a m e O b j e c t . G e t C o m p o n e n t < I A t t a c k a b l e > ( ) ;  
                 i f   ( a t t a c k a b l e   = =   n u l l )   r e t u r n ;  
  
                 G e t D a m a g e d ( a t t a c k a b l e . A t k ) ;  
         p u b l i c   v o i d   G e t D a m a g e d ( f l o a t   d a m a g e )  
         {  
                 H p   - =   d a m a g e ;  
  
         }  
                 s w i t c h   ( H p   /   M a x H p )  
                 {  
                         c a s e   < =   0 :  
                                 _ a n i m a t o r . S e t T r i g g e r ( " 0 % " ) ;  
                                 g a m e O b j e c t . S e t A c t i v e ( f a l s e ) ;  
                                 O n B l o c k D e s t r o y e d ? . I n v o k e ( ) ;  
                                 b r e a k ;  
                         c a s e   < =   0 . 3 3 :  
                                 _ a n i m a t o r . S e t T r i g g e r ( " 3 3 % " ) ;  
                                 b r e a k ;  
                         c a s e   < =   0 . 6 6 :  
                                 _ a n i m a t o r . S e t T r i g g e r ( " 6 6 % " ) ;  
                                 b r e a k ;  
                 }  
         }  
  
         p u b l i c   v o i d   I n i t i a l i z e ( A c t i o n < B l o c k >   r e t u r n A c t i o n )  
         {  
                 _ r e t u r n T o P o o l   =   r e t u r n A c t i o n ;  
         }  
  
         p u b l i c   v o i d   R e t u r n T o P o o l ( )  
         {  
                 _ r e t u r n T o P o o l ? . I n v o k e ( t h i s ) ;  
         }  
  
         p r i v a t e   v o i d   O n D i s a b l e ( )  
         {  
                 R e t u r n T o P o o l ( ) ;  
         }  
 }