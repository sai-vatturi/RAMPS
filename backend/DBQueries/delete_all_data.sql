-- ================================================
-- Comprehensive Data Deletion Script
-- ================================================

BEGIN TRANSACTION;

BEGIN TRY
    -- 1. Delete from ShoppingListItems (Child of ShoppingLists)
    DELETE FROM ShoppingListItems;

    -- 2. Delete from ShoppingLists (Child of MealPlans)
    DELETE FROM ShoppingLists;

    -- 3. Delete from MealPlanRecipes (Child of MealPlans and Recipes)
    DELETE FROM MealPlanRecipes;

    -- 4. Delete from Reviews (Child of Recipes)
    DELETE FROM Reviews;

    -- 5. Delete from Nutritions (Child of Recipes)
    DELETE FROM Nutritions;

    -- 6. Delete from Recipes (Child of Users)
    DELETE FROM Recipes;

    -- 7. Delete from MealPlans (Child of Users)
    DELETE FROM MealPlans;

    -- 8. Delete from Users (Parent Table)
    DELETE FROM Users;

    -- Commit the transaction if all deletions succeed
    COMMIT TRANSACTION;

    PRINT 'All data deleted successfully.';
END TRY
BEGIN CATCH
    -- Rollback the transaction in case of any error
    ROLLBACK TRANSACTION;

    -- Retrieve error information
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

    -- Raise the error to notify the user
    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH
