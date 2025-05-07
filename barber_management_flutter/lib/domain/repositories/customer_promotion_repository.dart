import 'package:barbermanagemobile/domain/entities/customer_promotion.dart';

abstract class CustomerPromotionRepository {
  Future<List<CustomerPromotion>> getCustomerPromotions(int customerId);
  Future<void> createCustomerPromotion(int customerId, int promoId, String status);
}